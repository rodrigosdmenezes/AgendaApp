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
        public string MedicoNome { get; set; }
        public Guid PacienteId { get; set; }
        public string PacienteNome { get; set; }
        public DateTime DataHora { get; set; }
        public bool EstaOcupada { get; set; }
    }
}