using WScoreDomain.Entities;
using WScoreInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WScoreBusiness
{
    public class CheckinService : ICheckinService
    {
        private readonly AppDbContext _context;

        public CheckinService(AppDbContext context)
        {
            _context = context;
        }

        public List<Checkin> ListarTodos()
            => _context.Checkins
                .Include(c => c.User)
                .OrderByDescending(c => c.DataCheckin)
                .ToList();

        public List<Checkin> ListarPaginado(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return _context.Checkins
                .AsNoTracking()
                .Include(c => c.User)
                .OrderByDescending(c => c.DataCheckin)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<Checkin> ListarPorUsuario(Guid userId)
            => _context.Checkins
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.DataCheckin)
                .ToList();

        public Checkin? ObterPorId(Guid id)
            => _context.Checkins
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);

        public Checkin Criar(Checkin c)
        {
            c.Score = CalcularScore(c);
            c.Feedback = GerarFeedback(c);

            _context.Checkins.Add(c);
            _context.SaveChanges();
            return c;
        }

        public bool Atualizar(Checkin c)
        {
            var existente = _context.Checkins.Find(c.Id);
            if (existente == null)
                return false;

            existente.Humor = c.Humor;
            existente.Sono = c.Sono;
            existente.Foco = c.Foco;
            existente.Energia = c.Energia;
            existente.CargaTrabalho = c.CargaTrabalho;

            existente.Score = CalcularScore(existente);
            existente.Feedback = GerarFeedback(existente);

            _context.SaveChanges();
            return true;
        }

        public bool Remover(Guid id)
        {
            var existente = _context.Checkins.Find(id);
            if (existente == null) return false;

            _context.Checkins.Remove(existente);
            _context.SaveChanges();
            return true;
        }

        private int CalcularScore(Checkin c)
        {
            decimal humorPeso = 0.25m;
            decimal energiaPeso = 0.25m;
            decimal sonoPeso = 0.30m;  
            decimal focoPeso = 0.10m;
            decimal cargaPeso = 0.10m;

            decimal sonoTransformado = 10 - c.Sono;

            decimal score =
                (c.Humor * humorPeso) +
                (c.Energia * energiaPeso) +
                (sonoTransformado * sonoPeso) +
                (c.Foco * focoPeso) +
                ((10 - c.CargaTrabalho) * cargaPeso);

            int finalScore = (int)(score * 10); 

            return Math.Clamp(finalScore, 0, 1000);
        }

        private string GerarFeedback(Checkin c)
        {
            List<string> alertas = new();

            if (c.Sono > 7)
                alertas.Add("Seu nível de sono está elevado. Tente priorizar pausas ou uma noite de descanso mais eficiente.");

            if (c.Humor <= 4)
                alertas.Add("Seu humor está baixo. Uma pausa curta ou alguma atividade leve pode ajudar.");

            if (c.Energia <= 5)
                alertas.Add("Seu nível de energia está reduzido. Considere hidratação ou alongamentos rápidos.");

            if (c.Foco <= 4)
                alertas.Add("Seu foco está comprometido. Talvez seja um bom momento para reorganizar prioridades.");

            if (c.CargaTrabalho >= 10)
                alertas.Add("Sua carga de trabalho está muito alta. Tente redistribuir atividades ou pedir apoio.");

            // Se não houver alertas:
            if (!alertas.Any())
                return "Seu check-in está equilibrado! Continue mantendo esse ritmo saudável e consistente.";

            return string.Join(" ", alertas);
        }
    }
}
