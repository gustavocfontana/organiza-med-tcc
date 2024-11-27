using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Compartilhado;

public class Medico : EntidadeBase
{
    protected Medico() {} // EF

    public string Nome { get; set; }
    public string Crm { get; set; }
    public List<Atividade> Atividades { get; set; } = new List<Atividade>();

    public Medico(string nome, string crm)
    {
        Nome = nome;
        Crm = crm;
    }

    // Novo construtor
    public Medico(int id, string nome, string crm, List<Atividade> atividades)
    {
        Id = id;
        Nome = nome;
        Crm = crm;
        Atividades = atividades ?? new List<Atividade>();
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (Nome.Length < 3)
            erros.Add("Nome inválido");
        if (Crm.Length < 8)
            erros.Add("CRM inválido");

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