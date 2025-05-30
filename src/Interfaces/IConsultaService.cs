using System;
using AgendaApp.src.Dtos;

namespace AgendaApp.src.Interfaces
{
    public interface IConsultaService
    {
        Task<ConsultaResponse> CriarConsultaAsync(ConsultaCreateRequest request);
        Task<IEnumerable<ConsultaResponse>> ListarConsultasAsync();
        Task<ConsultaResponse> ObterConsultaPorIdAsync(Guid id);
        Task<bool> CancelarConsultaAsync(Guid consultaId, Guid usuarioId, string tipoUsuario);
    }
}