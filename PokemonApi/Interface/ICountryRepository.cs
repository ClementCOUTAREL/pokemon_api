using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int countryId);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnerFromCountry(int countryId);
        Task<bool> isCountryExists(int countryId);
        Task<bool> CreateCountry (Country country);
        Task<bool> UpdateCountry(Country country);
        Task<bool> DeleteCountry(Country country);
        Task<bool> Save();
    }
}
