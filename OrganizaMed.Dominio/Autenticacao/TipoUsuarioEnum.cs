using System.ComponentModel.DataAnnotations;

public enum TipoUsuarioEnum
{
    Clinica,
    [Display(Name = "Funcionário")] Funcionario
}
