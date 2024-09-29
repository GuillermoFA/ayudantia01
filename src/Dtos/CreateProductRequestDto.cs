using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos
{
    public class CreateProductRequestDto
    {
        [Required]
        [MaxLength(366, ErrorMessage = "Name cannot be longer than 366 characters")]
        [MinLength(4, ErrorMessage = "Name cannot be shorter than 4 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be between 1 and 1000000")]
        public int Price { get; set; }
    }
}