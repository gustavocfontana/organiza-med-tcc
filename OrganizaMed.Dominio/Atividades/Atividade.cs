using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Dominio.Atividades
{
    public class Atividade : EntidadeBase
    {
        protected Atividade() {} // EF

        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Medico> MedicosEnvolvidos { get; set; } = new List<Medico>();
        public TimeSpan RecoveryTime { get; }

        public Atividade(string descricao, DateTime dataInicio, DateTime dataFim)
        {
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        // Novo construtor
        public Atividade(int id, string descricao, DateTime dataInicio, DateTime dataFim, List<Medico> medicosEnvolvidos)
        {
            Id = id;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
            MedicosEnvolvidos = medicosEnvolvidos ?? new List<Medico>();
        }

        public override List<string> Validar()
        {
            List<string> erros = new List<string>();

            if (Descricao.Length < 3)
                erros.Add("Descrição inválida");
            if (DataInicio > DataFim)
                erros.Add("Data de início maior que data de fim");

            return erros;
        }
    }
}