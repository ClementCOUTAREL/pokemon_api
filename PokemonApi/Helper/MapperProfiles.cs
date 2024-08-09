using AutoMapper;
using PokemonApi.Dto;
using PokemonApi.Models;

namespace PokemonApi.Helper
{
    public class MapperProfiles : Profile 
    {
        public MapperProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
