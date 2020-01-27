using System.ComponentModel.DataAnnotations;

namespace Supermarket.API.Resources
{
    public class SaveCategoryDto
    {
        [Required]
        [MaxLength(30)]
        public string Name {get; set; }
        public int id { get; set; }
    }
}