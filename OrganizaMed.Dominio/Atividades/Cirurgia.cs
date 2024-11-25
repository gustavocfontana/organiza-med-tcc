namespace OrganizaMed.Dominio.Atividades;

public class Cirurgia : Atividade
{
    public TimeSpan RecoveryTime => TimeSpan.FromHours(4);

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("Descrição da cirurgia é obrigatória");
        if (DataInicio == default)
            erros.Add("Data de início da cirurgia é obrigatória");
        if (DataFim == default)
            erros.Add("Data de fim da cirurgia é obrigatória");
        if (DataInicio > DataFim)
            erros.Add("Data de início da cirurgia não pode ser maior que a data de fim");
        if (MedicosEnvolvidos.Count == 0)
            erros.Add("É necessário informar pelo menos um médico envolvido na cirurgia");

        return erros;
    }
}