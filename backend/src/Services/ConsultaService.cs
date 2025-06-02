using AgendaApp.Entities;
using AgendaApp.Infra.Data;
using AgendaApp.src.Dtos;
using AgendaApp.src.Interfaces;
using backend.src.Dtos;
using Microsoft.EntityFrameworkCore;


namespace AgendaApp.src.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly ApplicationDbContext _context;

        public ConsultaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // MÉTODO PARA O MÉDICO DISPONIBILIZAR HORÁRIO
        public async Task<ConsultaResponse> DisponibilizarHorarioAsync(ConsultaCreateRequest request)
        {
            var consultaExiste = await _context.Consultas.FirstOrDefaultAsync(c =>
                c.MedicoId == request.MedicoId &&
                c.DataHora == request.DataHora);

            if (consultaExiste != null)
                throw new Exception("Já existe um horário cadastrado para esse médico e data.");

            var medico = await _context.Medicos.FindAsync(request.MedicoId)
                ?? throw new Exception("Médico não encontrado");

            var consulta = new Consulta
            {
                Id = Guid.NewGuid(),
                MedicoId = request.MedicoId,
                PacienteId = null,
                DataHora = request.DataHora,
                Status = ConsultaStatus.Disponivel,
                Medico = medico,
                Paciente = null
            };

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return MapToResponse(consulta);
        }

        // MÉTODO PARA O PACIENTE AGENDAR UMA CONSULTA
        public async Task<ConsultaResponse> AgendarConsultaAsync(ConsultaCreateRequest request, Guid pacienteId)
        {
            var dataHoraUtc = DateTime.SpecifyKind(request.DataHora, DateTimeKind.Utc);

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c =>
                    c.MedicoId == request.MedicoId &&
                    c.DataHora == dataHoraUtc &&
                    c.Status == ConsultaStatus.Disponivel);

            if (consulta == null)
                throw new Exception("Consulta não disponível.");

            consulta.PacienteId = null; // Se não for usar ID de paciente agora
            consulta.Status = ConsultaStatus.Agendada;

            await _context.SaveChangesAsync();

            var nomeMedico = await _context.Medicos
                .Where(u => u.Id == consulta.MedicoId)
                .Select(u => u.Nome)
                .FirstOrDefaultAsync();

            return new ConsultaResponse
            {
                Id = consulta.Id,
                MedicoId = consulta.MedicoId,
                NomeMedico = nomeMedico,
                PacienteId = pacienteId,
                DataHora = consulta.DataHora,
                Status = consulta.Status
            };
        }
        // BUSCAR CONSULTA POR ID
        public async Task<ConsultaResponse> BuscarPorIdAsync(Guid id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                throw new Exception("Consulta não encontrada.");

            return MapToResponse(consulta);
        }

        // LISTAR TODAS AS CONSULTAS
        public async Task<IEnumerable<ConsultaResponse>> ListarTodasAsync()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .ToListAsync();

            return consultas.Select(c => MapToResponse(c));
        }

        public async Task<IEnumerable<MedicoDisponivelDto>> ObterMedicosComHorariosDisponiveisAsync()
        {
            var medicosComHorarios = await _context.Consultas
                .Where(c => c.Status == ConsultaStatus.Disponivel)
                .Include(c => c.Medico)
                .GroupBy(c => new { c.MedicoId, c.Medico.Nome, c.Medico.Especialidade})
                .Select(g => new MedicoDisponivelDto
                {
                    MedicoId = g.Key.MedicoId,
                    Nome = g.Key.Nome,
                    Especialidade = g.Key.Especialidade,
                    HorariosDisponiveis = g.Select(c => c.DataHora).OrderBy(dt => dt).ToList()
                })
                .ToListAsync();

            return medicosComHorarios;
        }


        // CANCELAR CONSULTA
        public async Task CancelarConsultaAsync(Guid consultaId, Guid usuarioId, string tipoUsuario)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == consultaId);

            if (consulta == null)
                throw new Exception("Consulta não encontrada.");

            // Verifica se quem está cancelando é o médico ou o paciente da consulta
            if (tipoUsuario == "Medico" && consulta.MedicoId != usuarioId)
                throw new Exception("Médico não autorizado a cancelar esta consulta.");

            if (tipoUsuario == "Paciente" && consulta.PacienteId != usuarioId)
                throw new Exception("Paciente não autorizado a cancelar esta consulta.");

            // Só pode cancelar se a consulta estiver agendada
            if (consulta.Status != ConsultaStatus.Agendada)
                throw new Exception("Consulta não pode ser cancelada.");

            consulta.Status = ConsultaStatus.Cancelada;

            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
        }

        // MÉTODO AUXILIAR PARA MAPEAR ENTIDADE PARA RESPONSE DTO
        private ConsultaResponse MapToResponse(Consulta consulta)
        {
            return new ConsultaResponse
            {
                Id = consulta.Id,
                MedicoId = consulta.MedicoId,
                NomeMedico = consulta.Medico?.Nome,
                PacienteId = consulta.PacienteId ?? Guid.Empty,
                NomePaciente = consulta.Paciente?.Nome,
                DataHora = consulta.DataHora,
                Status = consulta.Status
            };
        }

        public Task<IEnumerable<ConsultaResponse>> ListarConsultasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ConsultaResponse> ObterConsultaPorIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IConsultaService.CancelarConsultaAsync(Guid consultaId, Guid usuarioId, string tipoUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
