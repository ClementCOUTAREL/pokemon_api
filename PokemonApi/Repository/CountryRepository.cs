﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _context.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerFromCountry(int countryId)
        {
            return _context.Countries.Where(c => c.Id == countryId).Select(o => o.Owner ).FirstOrDefault();
        }

        public bool isCountryExists(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        
    }
}
