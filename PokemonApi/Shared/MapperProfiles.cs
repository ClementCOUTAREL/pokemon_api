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
            CreateMap<Pokemon, GetPokemonRequest>().ReverseMap();
            CreateMap<Pokemon, UpdatePokemonRequest>().ReverseMap();
            CreateMap<Pokemon, CreatePokemonRequest>().ReverseMap();

            CreateMap<Category, GetCategoryRequest>().ReverseMap();
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, UpdatePokemonRequest>().ReverseMap();

            CreateMap<Country, GetCountryRequest>().ReverseMap();
            CreateMap<Country, CreateCountryRequest>().ReverseMap();
            CreateMap<Country, UpdateCountryRequest>().ReverseMap();

            CreateMap<Owner, GetOwnerRequest>().ReverseMap();
            CreateMap<Owner, CreateOwnerRequest>().ReverseMap();
            CreateMap<Owner, UpdateOwnerRequest>().ReverseMap();

            CreateMap<Review, GetReviewRequest>().ReverseMap();
            CreateMap<Review, CreateReviewRequest>().ReverseMap();
            CreateMap<Review, UpdateReviewRequest>().ReverseMap();

            CreateMap<Reviewer, GetReviewerRequest>().ReverseMap();
            CreateMap<Reviewer, CreateReviewerRequest>().ReverseMap();
            CreateMap<Reviewer, UpdateReviewerRequest>().ReverseMap();

        }
    }
}
