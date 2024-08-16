namespace PokemonApi.Dto.Pokemon
{
    public class GetPokemonRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}
