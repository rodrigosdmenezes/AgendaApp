using System;
using AgendaApp.src.Dtos;
using backend.src.Dtos;

namespace AgendaApp.src.Interfaces
{
    public interface IConsultaService
    {
        Task<ConsultaResponse> DisponibilizarHorarioAsync(ConsultaCreateRequest request);
        Task<ConsultaResponse> ObterConsultaPorIdAsync(Guid id);
        Task<IEnumerable<ConsultaResponse>> ListarConsultasAsync();
        Task<bool> CancelarConsultaAsync(Guid consultaId, Guid usuarioId, string tipoUsuario);
        Task<ConsultaResponse> AgendarConsultaAsync(ConsultaCreateRequest request, Guid pacienteId);
        Task<IEnumerable<MedicoDisponivelDto>> ObterMedicosComHorariosDisponiveisAsync();
    }
}