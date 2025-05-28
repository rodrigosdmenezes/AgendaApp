using System;
using System.Collections.Generic;

namespace AgendaApp.Entities
{
    public class Consulta
    {
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ocupado { get; set; }

        public Guid MedicoId { get; set; }
        public Medico Medico { get; set; }

        public Guid? PacienteId { get; set; }
        public Paciente Paciente { get; set; }
    }
}
