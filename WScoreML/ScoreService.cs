using Microsoft.ML;
using Microsoft.ML.Data;

namespace WScoreML;

public class CheckinML
{
    public float Humor { get; set; }
    public float Disposicao { get; set; }
    public float HorasDormidas { get; set; }
    public float Foco { get; set; }
    public float HorasTrabalhadas { get; set; }
    public float Score { get; set; } // usado em treinamento
}

public class ScorePrediction
{
    [ColumnName("Score")]
    public float Score { get; set; }
}

public static class ScoreService
{
    private static readonly MLContext mlContext = new();

    public static float CalcularScore(CheckinML checkin)
    {
        // Tenta usar modelo treinado
        if (File.Exists("wscore_model.zip"))
        {
            var model = mlContext.Model.Load("wscore_model.zip", out _);
            var predEngine = mlContext.Model.CreatePredictionEngine<CheckinML, ScorePrediction>(model);
            return predEngine.Predict(checkin).Score;
        }

        // Cálculo padrão
        double humor = checkin.Humor / 10.0;
        double disp = checkin.Disposicao / 10.0;
        double foco = checkin.Foco / 10.0;
        double sono = Math.Max(0, 1 - Math.Abs(checkin.HorasDormidas - 7) / 7);
        double trab = Math.Max(0, 1 - Math.Max(0, (checkin.HorasTrabalhadas - 9) / 7));

        return (float)((humor * 0.25 + disp * 0.25 + foco * 0.25 + sono * 0.15 + trab * 0.10) * 1000);
    }

    public static string GerarMensagem(float score, CheckinML c)
    {
        if (score >= 800) return "Excelente equilíbrio! Continue sua rotina atual.";
        if (score >= 600) return "Bom desempenho, mas tente dormir um pouco mais.";
        if (score >= 400) return "Atenção: reorganize o dia e tente descansar melhor.";
        return "Risco alto de burnout. Diminua o ritmo e priorize pausas longas.";
    }
}
