using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using AutoMapper;
using Supermarket.API.Resources;
using Supermarket.API.Extensions;
using System.Web.Http.Cors;


//controllers expose endpoints that allow outside programs to access api via http requests
namespace Supermarket.API.Controllers
{
    //truncates controller from class name below
    
    [EnableCors(origins: "http://localhost:3000",headers:"*",methods:"*")]
    [Route("/api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IMapper _mapper; //auto mapper allows you to convert entities into other classes such as dtos or resources
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {

            var categories = await _categoryService.ListAsync();
            //changes categories into category service. This limits what fields are returned.
            //entities match the columns in the database and could potentially return more data than needed
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryDto resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryDto, Category>(resource);
            var result = await _categoryService.SaveAsync(category);

            if (!result.Success)
                return BadRequest(result.Message);

            var CategoryDto = _mapper.Map<Category, CategoryDto>(result.Category);
            return Ok(CategoryDto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryDto resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryDto,Category>(resource);
            var result = await _categoryService.UpdateAsync(id, category);
            
            if (!result.Success)
                return BadRequest(result.Message);

            var CategoryDto = _mapper.Map<Category, CategoryDto>(result.Category);
            return Ok(CategoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);
            
            var CategoryDto = _mapper.Map<Category, CategoryDto>(result.Category);
            return Ok(CategoryDto);
        }
    }
}