using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonOfAnOwner(int ownerId);
        bool isOwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool Save();
    }
}
