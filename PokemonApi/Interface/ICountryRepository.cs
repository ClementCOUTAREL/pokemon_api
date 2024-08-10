using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnerFromCountry(int countryId);
        bool isCountryExists(int countryId);
        bool CreateCountry (Country country);
        bool Save();
    }
}
