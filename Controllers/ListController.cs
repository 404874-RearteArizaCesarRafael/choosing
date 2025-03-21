using choosing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace choosing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;

        public ListController(IListService listService)
        {
            _listService = listService;
        }
        // Obtener todos los invitados
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllInvitados()
        {
            var guests = await _listService.GetAllInvitadosAsync();
            return Ok(guests);
        }

        // Buscar por appelido o nombre
        [HttpGet("searchByName")]
        public async Task<IActionResult> SearchInvitado([FromQuery] string query)
        {
            var invitados = await _listService.SearchInvitadoAsync(query);
            return Ok(invitados);
        }

        // Buscar por dni
        [HttpGet("searchByDni")]
        public async Task<IActionResult> GetInvitadoByDni([FromQuery] int dni)
        {
            var invitado = await _listService.GetInvitadoByDniAsync(dni);
            return Ok(invitado);
        }

        // Acreditar un invitado
        [HttpPut("acreditar/{dni}")]
        public async Task<IActionResult> AcreditarInvitado(int dni)
        {
            var guest = await _listService.GetAllInvitadosAsync();
            var invitadoToAcreditar = guest.FirstOrDefault(i => i.Dni == dni);

            if (invitadoToAcreditar == null)
                return NotFound();

            await _listService.AcreditarInvitadoAsync(invitadoToAcreditar);
            return Ok(invitadoToAcreditar);
        }
    }
}
