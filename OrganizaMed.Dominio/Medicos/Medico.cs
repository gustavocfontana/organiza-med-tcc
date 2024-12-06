using System.Text.RegularExpressions;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Compartilhado;

public class Medico : EntidadeBase
{
    protected Medico() {} // EF

    public string Nome { get; set; }
    public string Crm { get; set; }
    public string Especialidade { get; set; }
    public List<Atividade> Atividades { get; set; } = new List<Atividade>();
    public double HorasTrabalhadas { get; set; } 
    public int Ranking { get; set; }

    public Medico(string nome, string crm, string especialidade)
    {
        Nome = nome;
        Crm = crm;
        Especialidade = especialidade;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (Nome.Length < 3)
            erros.Add("Nome inválido");
        if (Crm.Length < 8)
            erros.Add("CRM inválido");

        var crmRegex = new Regex(@"^\d{5}-[A-Z]{2}$");
        if (!crmRegex.IsMatch(Crm))
            erros.Add("CRM inválido. Deve ser no formato 12345-XX");

        if (string.IsNullOrWhiteSpace(Especialidade))
            erros.Add("Especialidade inválida");

        return erros;
    }

    public bool EstaDisponivel(DateTime dataInicio, DateTime dataFim)
    {
        foreach (var atividade in Atividades)
        {
            var fimComRecuperacao = atividade.DataFim + atividade.ObterTempoRecuperacao();

            // Verifica se há sobreposição de horários
            if (dataInicio < fimComRecuperacao && atividade.DataInicio < dataFim)
            {
                return false;
            }
        }
        return true;
    }

    public void AdicionarAtividade(Atividade atividade)
    {
        if (!EstaDisponivel(atividade.DataInicio, atividade.DataFim))
            throw new Exception("Médico não disponível");

        Atividades.Add(atividade);
    }

    public void CalcularHorasTrabalhadas()
    {
        HorasTrabalhadas = Atividades.Sum(a => (a.DataFim - a.DataInicio).TotalHours);
    }

    public void AtualizarRanking(List<Medico> medicos)
    {
        // Calcular horas trabalhadas para todos os médicos
        foreach (var medico in medicos)
        {
            medico.CalcularHorasTrabalhadas();
        }

        // Ordenar médicos por horas trabalhadas em ordem decrescente
        var medicosOrdenados = medicos.OrderByDescending(m => m.HorasTrabalhadas).ToList();

        // Atualizar ranking
        int ranking = 1;
        for ( int i = 0 ; i < medicosOrdenados.Count ; i++ )
        {
            if (i > 0 && medicosOrdenados[i].HorasTrabalhadas < medicosOrdenados[i - 1].HorasTrabalhadas)
            {
                ranking = i + 1;
            }
            medicosOrdenados[i].Ranking = ranking;
        }
    }
}
