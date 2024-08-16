namespace PokemonApi.Dto.Pokemon
{
    public class CreatePokemonRequest
    {
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}
