using System.ComponentModel.DataAnnotations;
using Supermarket.API.Domain.Models;

namespace Supermarket.API.Resources
{
    public class SaveProductDto
    {
        [Required]
        [MaxLength(30)]
        public string Name{get;set;}
        public int id {get;set;}
        public int QuantityInPackage{get; set;}
        public EUnitOfMeasurement UnitOfMeasurement {get; set;}
        public int CategoryId{get; set;}


    }
}