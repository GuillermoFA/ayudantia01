using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos.Users;
using api.src.Models;

namespace api.src.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Rut = userModel.Rut,
                Name = userModel.Name,
                Email = userModel.Email,
                NameRol = userModel.Role.Name,
                Products = userModel.Products.Select(p => p.ToProductDto()).ToList()

            };

        }
        
    }
}