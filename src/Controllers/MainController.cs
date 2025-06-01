using AgendaApp.src.Dtos;
using AgendaApp.src.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AgendaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConsultaService _consultaService;

        private Guid GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdString);
        }

        public MainController(IAuthService authService, IConsultaService consultaService)
        {
            _authService = authService;
            _consultaService = consultaService;
        }

        // ENDPOINTS PARA AUTENTICAÇÃO E LOGIN
        [HttpPost("register/paciente")]
        public async Task<IActionResult> RegisterPaciente([FromBody] RegisterPacienteRequest request)
        {
            var result = await _authService.RegisterPacienteAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        [HttpPost("register/medico")]
        public async Task<IActionResult> RegisterMedico([FromBody] RegisterMedicoRequest request)
        {
            var result = await _authService.RegisterMedicoAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        // ENDPOINTS PARA AS CONSULTAS
        [Authorize(Roles = "Medico")]
        [HttpPost("consulta/disponibilizar")]
        public async Task<IActionResult> DisponibilizarHorario([FromBody] ConsultaCreateRequest request)
        {
            var medicoId = GetUserId();
            request.MedicoId = medicoId;
            var response = await _consultaService.DisponibilizarHorarioAsync(request);
            return Ok(response);
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost("consulta")]
        public async Task<IActionResult> CriarConsulta([FromBody] ConsultaCreateRequest request)
        {
            var pacienteId = GetUserId();
            var result = await _consultaService.AgendarConsultaAsync(request, pacienteId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("consulta")]
        public async Task<IActionResult> ListarConsultas()
        {
            var result = await _consultaService.ListarConsultasAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("consulta/cancelar/{id}")]
        public async Task<IActionResult> CancelarConsultas(Guid id)
        {
            var usuarioId = Guid.Parse(User.FindFirst("id").Value);
            var tipoUsuario = User.FindFirst("tipoUsuario").Value;

            var result = await _consultaService.CancelarConsultaAsync(id, usuarioId, tipoUsuario);
            if (!result)
                return BadRequest("Não foi possível cancelar a consulta.");

            return NoContent();
        }
    }
}
