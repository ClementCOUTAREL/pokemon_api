using AutoMapper;
using PokemonApi.Dto.Category;
using PokemonApi.Dto.Country;
using PokemonApi.Dto.Owner;
using PokemonApi.Dto.Pokemon;
using PokemonApi.Dto.Review;
using PokemonApi.Dto.Reviewer;
using PokemonApi.Models;

namespace PokemonApi.Helper
{
    public class MapperProfiles : Profile 
    {
        public MapperProfiles()
        {
            CreateMap<Pokemon, PokemonDto>().ReverseMap();
            CreateMap<Pokemon, PokemonToUpdateDto>().ReverseMap();
            
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();

            CreateMap<Country, CountryDto>().ReverseMap();
            
            CreateMap<Owner, OwnerDto>().ReverseMap();
           
            CreateMap<Review, ReviewDto>().ReverseMap();
            
            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
            
        }
    }
}
