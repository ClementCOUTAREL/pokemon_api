using PokemonApi.Models;

namespace PokemonApi.Dto.Pokemon
{
    public class UpdatePokemonRequest
    {
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }

    }
}
