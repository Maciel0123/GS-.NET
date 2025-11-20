using WScoreDomain.Entities;
using WScoreInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WScoreBusiness;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<User> ListarTodos() =>
        _context.Users.AsNoTracking().ToList();

    public List<User> ListarPaginado(int page, int pageSize)
    {
        return _context.Users
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public User? ObterPorId(Guid id)
        => _context.Users.FirstOrDefault(x => x.Id == id);

    public User Criar(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public bool Atualizar(User user)
    {
        var existente = _context.Users.Find(user.Id);
        if (existente == null) return false;

        existente.Nome = user.Nome;
        existente.Email = user.Email;

        _context.SaveChanges();
        return true;
    }

    public bool Remover(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }
}
