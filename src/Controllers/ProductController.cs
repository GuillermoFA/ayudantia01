using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Dtos;
using api.src.Helpers;
using api.src.Interfaces;
using api.src.Mappers;
using api.src.Models;
using api.src.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cloudinary _cloudinary;
        public ProductController(IProductRepository productRepository, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await _productRepository.GetAll(query);
            var productDto = products.Select(p => p.ToProductDto());
            return Ok(productDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] CreateProductRequestDto request)
        {
            if(request.Image == null || request.Image.Length == 0)
            {
                return BadRequest("Image is required");
            }

            if(request.Image.ContentType  != "image/jpeg" && request.Image.ContentType != "image/png")
            {
                return BadRequest("Image must be a jpeg or png");
            }

            if(request.Image.Length > 10 * 1024 * 1024)
            {
                return BadRequest("Image must be less than 10mb");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.Image.FileName, request.Image.OpenReadStream()),
                Folder= "products_image"
            };

            var uploadReulst = await _cloudinary.UploadAsync(uploadParams);

            if(uploadReulst.Error != null)
            {
                return BadRequest(uploadReulst.Error.Message);
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                ImageUrl = uploadReulst.SecureUrl.AbsoluteUri
            };

            await _productRepository.Post(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProductRequestDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ProductModel = await _productRepository.Put(id, updateDto);
            if (ProductModel == null)
            {
                return NotFound();
            }
            return Ok(ProductModel.ToProductDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await _productRepository.Delete(id);
            if (product == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}