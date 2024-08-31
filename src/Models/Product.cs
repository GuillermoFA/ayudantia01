using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Models
{
    public class Product
    {
        public int Id { get; set; } // 1
        public string Name { get; set; } = string.Empty; // Arroz
        public int Price { get; set; }

        // Entity Framework Relationships
        public List<User> Users { get; set; } = [];  // pepito pedrito juan ...

    }
}