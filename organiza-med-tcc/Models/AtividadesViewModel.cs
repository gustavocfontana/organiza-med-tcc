using System.ComponentModel.DataAnnotations;

namespace organiza_med_tcc.Models
{
    public class InserirAtividadesViewModel
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MinLength(3, ErrorMessage = "A descrição deve conter ao menos 3 caracteres")]
        public string Descricao { get; set; }


        [Required(ErrorMessage = "A data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória")]

        public DateTime DataTermino { get; set; }

        [Required(ErrorMessage = "O médico é obrigatório")]
        public int MedicoId { get; set; }
    }

    public class EditarAtividadesViewModel
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MinLength(3, ErrorMessage = "A descrição deve conter ao menos 3 caracteres")]
        public string Descricao { get; set; }


        [Required(ErrorMessage = "A data de início é obrigatória")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória")]

        public DateTime DataTermino { get; set; }

        [Required(ErrorMessage = "O médico é obrigatório")]
        public int MedicoId { get; set; }
    }

    public class ListarAtividadesViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string Medico { get; set; }
    }

    public class DetalhesAtividadesViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string Medico { get; set; }
    }
}
