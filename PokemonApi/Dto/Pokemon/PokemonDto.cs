namespace PokemonApi.Dto.Pokemon
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}
