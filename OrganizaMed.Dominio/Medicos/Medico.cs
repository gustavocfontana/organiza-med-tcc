using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Compartilhado;

namespace OrganizaMed.Dominio.Medicos
{
    public class Medico : EntidadeBase
    {
        protected Medico() {} // EF

        public string Nome { get; set; }
        public string Crm { get; set; }
        public string Especialidade { get; set; }
        List<Atividade> Atividades { get; set; } = new List<Atividade>();

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

        public bool EstaDisponivel(DateTime dataInicio, DateTime dataFim)
        {
            return Atividades.All(a => a.DataFim < dataInicio || a.DataInicio > dataFim);
        }

        public void AdicionarAtividade(Atividade atividade)
        {
            if (!EstaDisponivel(atividade.DataInicio, atividade.DataFim))
                throw new Exception("Médico não disponível");

            Atividades.Add(atividade);
        }
    }
}
