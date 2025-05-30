using AgendaApp.src.Dtos;
using AgendaApp.src.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AgendaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConsultaService _consultaService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _consultaService ConsultaService;
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
        [Authorize]
        [HttpPost("consulta")]
        public async Task<IActionResult> CriarConsulta([FromBody] ConsultaCreateRequest request){
            var result = await _consultaService.CriarConsultaAsync(request);
            return Ok(result.Success);
        }

        [Authorize]
        [HttpGet("consulta")]
        public async Task<IActionResult>ListarConsultas(){
            var result = await _consultaService.ListarConsultasAsync();
            return Ok(result.Success);
        }
        
        [Authorize]
        [HttpPut("consulta/cancelar/{id}")]
        public async Task<IActionResult>CancelarConsultas(Guid id){
            await _consultaService.CancelarConsultasAsync(id);
            return NoContent();
        }
    }
}
