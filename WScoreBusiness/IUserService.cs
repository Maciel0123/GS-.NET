using WScoreDomain.Entities;

namespace WScoreBusiness
{
    public interface IUserService
    {
        List<User> ListarTodos();
        List<User> ListarPaginado(int page, int pageSize);
        User? ObterPorId(Guid id);
        User Criar(User user);
        bool Atualizar(User user);
        bool Remover(Guid id);
    }
}
