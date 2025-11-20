using WScoreDomain.Entities;

namespace WScoreBusiness
{
    public interface ICheckinService
    {
        List<Checkin> ListarTodos();
        List<Checkin> ListarPaginado(int page, int pageSize);
        List<Checkin> ListarPorUsuario(Guid userId);
        Checkin? ObterPorId(Guid id);
        Checkin Criar(Checkin checkin);
        bool Atualizar(Checkin checkin);
        bool Remover(Guid id);
    }
}
