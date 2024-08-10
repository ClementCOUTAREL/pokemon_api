using AutoMapper;
using PokemonApi.Dto;
using PokemonApi.Models;

namespace PokemonApi.Helper
{
    public class MapperProfiles : Profile 
    {
        public MapperProfiles()
        {
            CreateMap<Pokemon, PokemonDto>().ReverseMap();
            
            CreateMap<Category, CategoryDto>().ReverseMap();
            
            CreateMap<Country, CountryDto>().ReverseMap();
            
            CreateMap<Owner, OwnerDto>().ReverseMap();
           
            CreateMap<Review, ReviewDto>().ReverseMap();
            
            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
            
        }
    }
}
