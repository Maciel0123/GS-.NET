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
            decimal media =
                (c.Humor * 0.20m) +
                (c.Energia * 0.20m) +
                ((10 - c.Sono) * 0.20m) + // SONO inverso
                (c.Foco * 0.20m) +
                ((10 - c.CargaTrabalho) * 0.20m);

            c.Score = (int)(media * 100);

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

            decimal media =
                (existente.Humor * 0.20m) +
                (existente.Energia * 0.20m) +
                ((10 - existente.Sono) * 0.20m) + // SONO inverso
                (existente.Foco * 0.20m) +
                ((10 - existente.CargaTrabalho) * 0.20m);

            existente.Score = (int)(media * 100);

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
    }
}
