using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        public string Rut { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NameRol { get; set; } = string.Empty;
        public List<ProductDto> Products { get; set; } = [];

    }
}