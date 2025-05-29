using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaApp.src.Dtos
{
    public class ConsultaCreateRequest
    {
        public Guid MedicoId { get; set; }
        public Guid PacienteId { get; set; }
        public DateTime DataHora { get; set; }
    }
}