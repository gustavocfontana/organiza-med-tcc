using System.ComponentModel.DataAnnotations;

namespace organiza_med_tcc.Models
{
    public class InserirMedicosViewModel
    {
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "CRM")]
        [RegularExpression(@"^\d{5}-[A-Z]{2}$", ErrorMessage = "O CRM deve ser composto por cinco dígitos e a sigla do estado (ex: 78806-SP).")]
        public string Crm { get; set; }

        [Required]
        [Display(Name = "Especialidade")]
        public string Especialidade { get; set; }

        public List<Medico> Medicos { get; set; } = new List<Medico>();
    }

    public class EditarMedicosViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "CRM")]
        [RegularExpression(@"^\d{5}-[A-Z]{2}$", ErrorMessage = "O CRM deve ser composto por cinco dígitos e a sigla do estado (ex: 78806-SP).")]
        public string Crm { get; set; }
    }

    public class ListarMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public string Especialidade { get; set; }
    }

    public class DetalhesMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
        public string Especialidade { get; set; }
    }

    public class TopMedicosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double HorasTrabalhadas { get; set; }
    }
}
