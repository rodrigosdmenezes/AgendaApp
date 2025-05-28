using System;
using System.Collections.Generic;

namespace AgendaApp.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public List<Consulta> Consultas { get; set; }
    }
}