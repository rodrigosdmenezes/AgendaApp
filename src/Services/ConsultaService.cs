using AgendaApp.src.Data;
using AgendaApp.src.Dtos;
using AgendaApp.src.Entities;
using AgendaApp.src.Enums;
using AgendaApp.src.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ConsultaResponse> AgendarConsultaAsync(Guid consultaId, Guid pacienteId)
        {
            var consulta = await _context.Consultas.Include(c => c.Medico).Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == consultaId);

            if (consulta == null)
                throw new Exception("Consulta não encontrada.");

            if (consulta.Status != ConsultaStatus.Disponivel)
                throw new Exception("Consulta não está disponível para agendamento.");

            var paciente = await _context.Pacientes.FindAsync(pacienteId)
                ?? throw new Exception("Paciente não encontrado.");

            consulta.PacienteId = pacienteId;
            consulta.Status = ConsultaStatus.Agendada;
            consulta.Paciente = paciente;

            await _context.SaveChangesAsync();

            return MapToResponse(consulta);
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
                MedicoNome = consulta.Medico?.Nome,
                PacienteId = consulta.PacienteId ?? Guid.Empty,
                PacienteNome = consulta.Paciente?.Nome,
                DataHora = consulta.DataHora,
                EstaOcupada = consulta.Status == ConsultaStatus.Agendada
            };
        }
    }
}
