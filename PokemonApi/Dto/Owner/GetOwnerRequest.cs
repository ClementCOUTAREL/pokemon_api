﻿using PokemonApi.Models;

namespace PokemonApi.Dto.Owner
{
    public class GetOwnerRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
    }
}
