using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Dto.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
    }
}
