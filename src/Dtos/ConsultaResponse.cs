using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaApp.src.Dtos
{
    public class ConsultaResponse
    {   
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public Guid? PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataHora { get; set; }
        public ConsultaStatus Status { get; set; }
    }
}