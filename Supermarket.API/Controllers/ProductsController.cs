using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using AutoMapper;
using Supermarket.API.Resources;
using Supermarket.API.Extensions;
using System.Web.Http.Cors;

namespace Supermarket.API.Controllers
{
    [EnableCors(origins: "http://localhost:3000",headers:"*",methods:"*")]
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> ListAsync()
        {
            var products = await _productService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveProductDto resource)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var product = _mapper.Map<SaveProductDto, Product>(resource);
            var result = await _productService.SaveAsync(product);

            if (!result.Success)
                return BadRequest(result.Message);
            
            var productResource = _mapper.Map<Product, ProductDto>(result.Product);
            return Ok(productResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProductDto resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var product  = _mapper.Map<SaveProductDto,Product>(resource);
            var result = await _productService.UpdateAsync(id,product);

            if (!result.Success)
                return BadRequest(result.Message);

            var ProductDto = _mapper.Map<Product,ProductDto>(product);
            return Ok(ProductDto);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productService.DeleteAsync(id);
            
            if (!result.Success)
                return BadRequest(result.Message);

            var ProductDto = _mapper.Map<Product,ProductDto>(result.Product);
            return Ok(ProductDto);
        }
    }
}