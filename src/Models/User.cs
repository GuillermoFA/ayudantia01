using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Models
{
    public class User
    {
        public int Id { get; set; } // 1
        public string Rut { get; set; } = string.Empty; // 1124112
        public string Name { get; set; } = string.Empty; // Juan
        public string Email { get; set; } = string.Empty; // juancito

        // Entity Framework Relationships
        public List<Product> Products { get; set; } = []; // arroz leche pan

        public int RoleId { get; set; } // 1
        public Role Role { get; set; } = null!;
        
    }
}