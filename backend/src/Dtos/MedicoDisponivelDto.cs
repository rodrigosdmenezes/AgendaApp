using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.Dtos
{
    public class MedicoDisponivelDto
    {
        public Guid MedicoId { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public List<DateTime> HorariosDisponiveis { get; set; }
    }
}