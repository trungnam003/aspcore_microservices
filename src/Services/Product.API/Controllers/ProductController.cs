using AutoMapper;
using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetProductsAsync();
            var data = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(data);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProductByID([Required][FromRoute] long id)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var data = _mapper.Map<ProductDto>(product);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _repository.CreateProductAsync(product);
            await _repository.SaveAsync();
            var data = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetProductByID), new { id = data.Id }, data);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct([Required][FromRoute] long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var updateProduct = _mapper.Map(productDto, product);
            await _repository.UpdateProductAsync(updateProduct);
            await _repository.SaveAsync();
            var data = _mapper.Map<ProductDto>(product);
            return Ok(data);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([Required][FromRoute] long id)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _repository.DeleteProductAsync(id);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpGet("get-by-no/{No}")]
        public async Task<IActionResult> GetProductByNo([Required][FromRoute] string No)
        {
            var product = await _repository.GetProductByNoAsync(No);
            if (product == null)
            {
                return NotFound();
            }
            var data = _mapper.Map<ProductDto>(product);
            return Ok(data);
        }
        #endregion

    }
}
