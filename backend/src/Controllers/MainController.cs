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
        // Endpoint para realizar o login. Url: http://localhost:5074/api/main/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        //Endpoint para cadastrar paciente. Url: http://localhost:5074/api/main/register/paciente
        [HttpPost("register/paciente")]
        public async Task<IActionResult> RegisterPaciente([FromBody] RegisterPacienteRequest request)
        {
            var result = await _authService.RegisterPacienteAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        //Endpoint para cadastrar medico. Url: http://localhost:5074/api/main/register/medico
        [HttpPost("register/medico")]
        public async Task<IActionResult> RegisterMedico([FromBody] RegisterMedicoRequest request)
        {
            var result = await _authService.RegisterMedicoAsync(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Token);
        }

        // ENDPOINTS PARA AS CONSULTAS

        //Endpoint para os médicos disponibilizar as datas. Url: http://localhost:5074/api/main/consulta/disponibiliza
        [HttpPost("consulta/disponibilizar")]
        public async Task<IActionResult> DisponibilizarHorario([FromBody] ConsultaCreateRequest request)
        {
            var response = await _consultaService.DisponibilizarHorarioAsync(request);
            return Ok(response);
        }

        //Endpoint para os pacientes agendar as consultas. Url: http://localhost:5074/api/main/consulta
        [HttpPost("consulta")]
        public async Task<IActionResult> CriarConsulta([FromBody] ConsultaCreateRequest request)
        {
            var pacienteId = GetUserId();
            var result = await _consultaService.AgendarConsultaAsync(request, pacienteId);
            return Ok(result);
        }

        //Endpoint para trazer os médicos com hórarios disponiveis. Url: http://localhost:5074/api/main/medicos/disponiveis
        [HttpGet("medicos/disponiveis")]
        public async Task<IActionResult> ObterMedicosDisponiveis()
        {
            var medicos = await _consultaService.ObterMedicosComHorariosDisponiveisAsync();
            return Ok(medicos);
        }


        [Authorize]

        //Endpoint para exebir as consultas de cada usuário paciente. Url: http://localhost:5074/api/main/consulta/paciente/{pacienteId}
        [HttpGet("consulta/paciente/{pacienteId}")]
        public async Task<IActionResult> ListarConsultasPorPaciente(Guid pacienteId)
        {
            var result = await _consultaService.ListarPorPacienteAsync(pacienteId);
            return Ok(result);
        }

        //Endpoint para exebir as consultas agendadas para os médicos. Url: http://localhost:5074/api/main/consulta/medico/{medicoId}
        [HttpGet("consulta/medico/{medicoId}")]
        public async Task<IActionResult> ConsultasPorMedico(Guid medicoId)
        {
            var result = await _consultaService.ListarPorMedicoAsync(medicoId);
            return Ok(result);
        }

        //Endpoint para cancelar as consultas
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
