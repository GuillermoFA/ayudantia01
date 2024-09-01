using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Models
{
    public class User
    {
        public int Id { get; set; } // 1
        public string Rut { get; set; } = string.Empty; // 2141241
        public string Name { get; set; } = string.Empty; // Juan
        public string Email { get; set; } = string.Empty;


        // Entityframework relationships
        public List<Product> Products { get; set; } = []; // arroz, azucar, leche, pan...

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!; // admin, user, guest...


    }
}