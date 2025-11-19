namespace WScoreDomain.Entities
{
    public class Checkin
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DataCheckin { get; set; } = DateTime.UtcNow;

        public int Humor { get; set; }
        public int Sono { get; set; }
        public int Foco { get; set; }

        public int Score { get; set; }

        public int Energia { get; set; }

        public int CargaTrabalho { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; } = default!;
    }
}
