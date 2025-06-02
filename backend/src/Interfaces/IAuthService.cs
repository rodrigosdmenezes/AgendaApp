using System;
using AgendaApp.src.Dtos;

namespace AgendaApp.src.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterPacienteAsync(RegisterPacienteRequest request);
        Task<AuthResult> RegisterMedicoAsync(RegisterMedicoRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
    }
}