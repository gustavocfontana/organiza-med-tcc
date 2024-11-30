using System.ComponentModel.DataAnnotations;
using FluentResults;

namespace organiza_med_tcc.Models
{
    public class InserirAtividadesViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public string TipoAtividade { get; set; }

        [Required]
        public List<int> MedicoId { get; set; }

        public IEnumerable<Medico> ? Medicos { get; set; }
    }

    public class EditarAtividadesViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public string TipoAtividade { get; set; }

        public IEnumerable<Medico> ? Medicos { get; set; }
    }

    public class MedicoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class ListarAtividadesViewModel
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public string TipoAtividade { get; set; }

        public List<int> MedicoId { get; set; }

        public IEnumerable<Medico> ? Medicos { get; set; }
    }

    public class DetalhesAtividadesViewModel
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TipoAtividade { get; set; }
    }
}
