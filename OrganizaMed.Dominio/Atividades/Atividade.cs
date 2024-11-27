using OrganizaMed.Dominio.Compartilhado;


namespace OrganizaMed.Dominio.Atividades
{
    public enum TipoAtividade
    {
        Consulta,
        Cirurgia
    }

    public class Atividade : EntidadeBase
    {
        protected Atividade() {} // EF

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<Medico> MedicosEnvolvidos { get; set; } = new List<Medico>();
        public TipoAtividade TipoAtividade { get; set; }

        public Atividade(DateTime dataInicio, DateTime dataFim, TipoAtividade tipoAtividade)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            TipoAtividade = tipoAtividade;
        }

        // Novo construtor
        public Atividade(int id, DateTime dataInicio, DateTime dataFim, List<Medico> medicosEnvolvidos, TipoAtividade tipoAtividade)
        {
            Id = id;
            DataInicio = dataInicio;
            DataFim = dataFim;
            MedicosEnvolvidos = medicosEnvolvidos ?? new List<Medico>();
            TipoAtividade = tipoAtividade;
        }

        public override List<string> Validar()
        {
            List<string> erros = new List<string>();

            if (DataInicio > DataFim)
                erros.Add("Data de início maior que data de fim");

            return erros;
        }

        public TimeSpan ObterTempoRecuperacao()
        {
            return TipoAtividade == TipoAtividade.Cirurgia ? TimeSpan.FromHours(4) : TimeSpan.FromMinutes(10);
        }
    }
}