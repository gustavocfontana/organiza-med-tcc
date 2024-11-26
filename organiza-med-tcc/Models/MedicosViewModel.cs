using System.ComponentModel.DataAnnotations;

namespace organiza_med_tcc.Models
{
    public class InserirMedicosViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome deve conter ao menos 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CRM é obrigatório")]
        [MinLength(3, ErrorMessage = "O CRM deve conter ao menos 8 caracteres")]
        public string Crm { get; set; }

    }

    public class EditarMedicosViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome deve conter ao menos 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CRM é obrigatório")]
        [MinLength(3, ErrorMessage = "O CRM deve conter ao menos 8 caracteres")]
        public string Crm { get; set; }

    }

    public class ListarMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
    }

    public class DetalhesMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
    }

    public class TopMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double HorasTrabalhadas { get; set; }
    }
}