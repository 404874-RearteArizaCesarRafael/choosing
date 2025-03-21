using choosing.Domain;
using choosing.Repository.Interfaces;
using choosing.Services.Interfaces;

namespace choosing.Services.Impl
{
    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;
        
        public ListService(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }
        public async Task AcreditarInvitadoAsync(Guest guest)
        {
            //invitado.Ingreso = true;  // Lógica de acreditación (puedes añadir más lógica aquí)
            guest.Acreditado = 1;
            await _listRepository.UpdateAsync(guest);
        }

        public async Task<List<Guest>> GetAllInvitadosAsync()
        {
            return await _listRepository.GetAllAsync();
        }

        public async Task<Guest?> GetInvitadoByDniAsync(int dni)
        {
            return await _listRepository.GetByDNIAsync(dni);
        }

        public async  Task<List<Guest>> SearchInvitadoAsync(string query)
        {
            return await _listRepository.SearchByNameAsync(query);

        }
    }
}
