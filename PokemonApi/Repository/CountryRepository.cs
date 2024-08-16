using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        async public Task<ICollection<Country>> GetCountries()
        {
            return await _context.Countries.OrderBy(c => c.Id).ToListAsync();
        }

        async public Task<Country> GetCountry(int countryId)
        {
            return await _context.Countries.Where(c => c.Id == countryId).FirstOrDefaultAsync();
        }

        async public Task<Country> GetCountryByOwner(int ownerId)
        {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Owner>> GetOwnerFromCountry(int countryId)
        {
            return await _context.Countries.Where(c => c.Id == countryId).Select(o => o.Owner ).FirstOrDefaultAsync();
        }

        async public Task<bool> isCountryExists(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }

        async public Task<bool> CreateCountry(Country country)
        {
            _context.Add(country);
            return await Save();
        }

        async public Task<bool> UpdateCountry(Country country)
        {
            _context.Update(country);
            return await Save();
        }

        async public Task<bool> DeleteCountry(Country country)
        {
            _context.Remove(country);
            return await Save();
        }

        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
