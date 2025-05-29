using AgendaApp.src.Dtos;
using AgendaApp.src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

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
    }
}
