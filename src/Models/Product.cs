using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.src.Models
{
    public class Product
    {
        // Atributos
        public int Id { get; set; } // 1 
        public string Name { get; set; } = string.Empty; // arroz
        public int Price { get; set; } // 1500
        public string ImageUrl { get; set; } = string.Empty; // https://cloudinary.com/1234

        // Entityframework relationships
    }
}