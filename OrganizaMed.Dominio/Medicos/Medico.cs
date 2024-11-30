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

    public Medico(string nome, string crm, string especialidade)
    {
        Nome = nome;
        Crm = crm;
        Especialidade = especialidade;
    }

    public Medico(int id, string nome, string crm, string especialidade, List<Atividade> atividades)
    {
        Id = id;
        Nome = nome;
        Crm = crm;
        Especialidade = especialidade;
        Atividades = atividades ?? new List<Atividade>();
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
        return Atividades.All(a => a.DataFim + a.ObterTempoRecuperacao() <= dataInicio || a.DataInicio >= dataFim);
    }

    public void AdicionarAtividade(Atividade atividade)
    {
        if (!EstaDisponivel(atividade.DataInicio, atividade.DataFim))
            throw new Exception("Médico não disponível");

        Atividades.Add(atividade);
    }
}