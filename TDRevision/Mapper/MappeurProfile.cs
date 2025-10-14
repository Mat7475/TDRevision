using AutoMapper;

using TDRevision.Models;
using TDRevision.Models.DTO;

namespace TDRevision.Mapper
{
    public class MappeurProfile : Profile
    {
        public MappeurProfile()
        {
            CreateMap<Utilisateur, UtilisateurDTO>()
                .ForMember(dest => dest.NbCommandes, opt => opt.MapFrom(src => src.Commandes.Count))
                .ReverseMap();

            CreateMap<Commande, CommandeDTO>()
                .ReverseMap();
        }
    }
}