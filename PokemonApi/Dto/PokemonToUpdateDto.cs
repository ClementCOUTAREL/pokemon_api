﻿using PokemonApi.Models;

namespace PokemonApi.Dto
{
    public class PokemonToUpdateDto
    {
            public string Name { get; set; } = null!;
            public DateTime BirthDate { get; set; }
 
    }
}
