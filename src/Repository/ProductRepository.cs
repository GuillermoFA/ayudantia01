using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Dtos;
using api.src.Helpers;
using api.src.Interfaces;
using api.src.Models;
using Microsoft.EntityFrameworkCore;

namespace api.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Product?> Delete(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<List<Product>> GetAll(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Name)) products = products.Where(x => x.Name.Contains(query.Name));

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDescending ? products.OrderByDescending(x => x.Name) : products.OrderBy(x => x.Name);
                }
            }


            return await products.ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Post(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> Put(int id, UpdateProductRequestDto productDto)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }
            productModel.Name = productDto.Name;
            productModel.Price = productDto.Price;

            await _context.SaveChangesAsync();
            return productModel;
        }
    }
}