using AutoMapper;
using organiza_med_tcc.Models;

namespace organiza_med_tcc.Mapping
{
    public class MedicosProfile : Profile
    {
        public MedicosProfile()
        {
            CreateMap<InserirMedicosViewModel, Medico>();
            CreateMap<EditarMedicosViewModel, Medico>();

            CreateMap<Medico, ListarMedicosViewModel>();
            CreateMap<Medico, DetalhesMedicosViewModel>();
            CreateMap<Medico, EditarMedicosViewModel>();
            CreateMap<Medico, TopMedicosViewModel>();
        }
    }
}
