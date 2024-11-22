using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizaMed.Dominio.Compartilhado;

namespace OrganizaMed.Dominio.Medicos
{
    public class Medico : EntidadeBase
    {
        protected Medico() {} // EF

        public string Nome { get; set; }
        public string Crm { get; set; }
        public string Especialidade { get; set; }

        public Medico(string nome, string crm, string especialidade)
        {
            Nome = nome;
            Crm = crm;
            Especialidade = especialidade;
        }

        public override List<string> Validar()
        {
            List<string> erros = [];

            if (Nome.Length < 3)
                erros.Add("Nome inválido");
            if (Crm.Length < 8)
                erros.Add("CRM inválido");

            return erros;
        }
    }
}
