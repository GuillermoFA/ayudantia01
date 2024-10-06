using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.Helpers;
using api.src.Models;

namespace api.src.Interfaces
{
    public interface IProductRepository
    {
        Task <List<Product>> GetAll(QueryObject query);
        Task<Product?> GetById(int id);
        Task<Product> Post(Product request);
        Task<Product?> Put(int id, UpdateProductRequestDto productDto);
        Task<Product?> Delete(int id);
    }
}