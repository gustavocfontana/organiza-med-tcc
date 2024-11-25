using OrganizaMed.Dominio.Atividades;

public class Consulta : Atividade
{
    public TimeSpan RecoveryTime => TimeSpan.FromMinutes(10);

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (DataInicio.Hour < 8 || DataInicio.Hour > 18)
            erros.Add("Horário inválido");

        return erros;
    }
}