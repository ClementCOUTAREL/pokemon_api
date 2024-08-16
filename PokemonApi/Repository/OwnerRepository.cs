using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        async public Task<ICollection<Owner>> GetOwners()
        {
            return await _context.Owners.OrderBy(o => o.Id).ToListAsync();
        }

        async public Task<Owner> GetOwner(int ownerId)
        {
            return await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Owner>> GetOwnerOfAPokemon(int pokeId)
        {
            return await _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToListAsync();
        }

        

        async public Task<ICollection<Pokemon>> GetPokemonOfAnOwner(int ownerId)
        {
            return await _context.PokemonOwners.Where(o => o.Owner.Id == ownerId).Select(p => p.Pokemon).ToListAsync();
        }

        async public Task<bool> isOwnerExists(int ownerId)
        {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }

        async public Task<bool> CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return await Save();
        }

       async public Task<bool> UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return await Save();
        }

        async public Task<bool> DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return await Save();
        }
        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
