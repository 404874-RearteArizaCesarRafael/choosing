using choosing.Domain;

namespace choosing.Services.Interfaces
{
    public interface IListService
    {
        Task<List<Guest>> GetAllInvitadosAsync();
        Task<List<Guest>> SearchInvitadoAsync(string query);
        Task<Guest?> GetInvitadoByDniAsync(int dni);
        Task AcreditarInvitadoAsync(Guest guest);
    }
}
