using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwners();
        Task<Owner> GetOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnerOfAPokemon(int pokeId);
        Task<ICollection<Pokemon>> GetPokemonOfAnOwner(int ownerId);
        Task<bool> isOwnerExists(int ownerId);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> UpdateOwner(Owner owner);
        Task<bool> DeleteOwner(Owner owner);
        Task<bool> Save();
    }
}
