using AutoMapper;
using organiza_med_tcc.Models;
using OrganizaMed.Dominio.Atividades;

namespace organiza_med_tcc.Mapping
{
    public class AtividadesProfile : Profile
    {
        public AtividadesProfile()
        {
            CreateMap<InserirAtividadesViewModel, Atividade>();
            CreateMap<EditarAtividadesViewModel, Atividade>();

            CreateMap<Atividade, ListarAtividadesViewModel>();
            CreateMap<Atividade, DetalhesAtividadesViewModel>();
            CreateMap<Atividade, EditarAtividadesViewModel>();
            CreateMap<Atividade, TopMedicosViewModel>();
        }
    }
}
