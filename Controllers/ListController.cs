using choosing.Domain;
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

        // Agregar un nuevo invitado
        [HttpPost("create")]
        public async Task<IActionResult> CreateInvitado([FromBody] Guest newGuest)
        {
            if (newGuest == null)
                return BadRequest("Datos de invitado inválidos");

            try
            {
                // Validar que el DNI no exista ya
                var existingGuest = await _listService.GetInvitadoByDniAsync(newGuest.Dni);
                if (existingGuest != null)
                    return Conflict($"Ya existe un invitado con el DNI {newGuest.Dni}");

                // Inicializar campos para un nuevo invitado
                newGuest.Acreditado = 0; // No acreditado inicialmente

                await _listService.CreateInvitadoAsync(newGuest);
                return CreatedAtAction(nameof(GetInvitadoByDni), new { dni = newGuest.Dni }, newGuest);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Error interno al crear invitado: {ex.Message}");
            }
        }
        // Actualizar un invitado
        [HttpPut("update/{originalDni}")]
        public async Task<IActionResult> UpdateInvitado(int originalDni, [FromBody] Guest updatedGuest)
        {
            if (updatedGuest == null)
                return BadRequest("Datos de invitado inválidos");

            try
            {
                // Obtener el invitado original
                var existingGuest = await _listService.GetInvitadoByDniAsync(originalDni);
                if (existingGuest == null)
                    return NotFound($"No se encontró un invitado con el DNI {originalDni}");

                // Si el DNI cambió, verificar que el nuevo DNI no exista
                if (originalDni != updatedGuest.Dni)
                {
                    var existingWithNewDni = await _listService.GetInvitadoByDniAsync(updatedGuest.Dni);
                    if (existingWithNewDni != null && existingWithNewDni.Dni != originalDni)
                        return Conflict($"Ya existe otro invitado con el DNI {updatedGuest.Dni}");
                }

                // Actualizar el invitado
                await _listService.UpdateInvitadoAsync(originalDni, updatedGuest);
                return Ok(updatedGuest);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Error interno al actualizar invitado: {ex.Message}");
            }
        }

        // Actualizar solo el estado de acreditación (endpoint específico para el toggle)
        [HttpPut("updateAccreditStatus/{dni}")]
        public async Task<IActionResult> UpdateAccreditStatus(int dni, [FromBody] AccreditStatusDto status)
        {
            try
            {
                var invitado = await _listService.GetInvitadoByDniAsync(dni);
                if (invitado == null)
                    return NotFound($"No se encontró un invitado con el DNI {dni}");

                // Actualizar solo el estado de acreditación
                invitado.Acreditado = status.Acreditado;

                await _listService.UpdateAccreditStatusAsync(invitado);
                return Ok(invitado);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Error interno al actualizar estado de acreditación: {ex.Message}");
            }
        }
    }
}
