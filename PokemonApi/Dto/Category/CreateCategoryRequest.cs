using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Dto.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
