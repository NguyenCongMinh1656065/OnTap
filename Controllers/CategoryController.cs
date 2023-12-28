using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ôn_tập.Data;
using Ôn_tập.Dto;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;
using Ôn_tập.Repository;

namespace Ôn_tập.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IMapper mapper , ICategoryRepository categoryRepository) 
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id) 
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound();
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }
        [HttpGet("/pokemon/{id}")]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(
                _categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }
        [HttpPut("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId , [FromBody] CategoryDto updateCategory)
        {
            if (updateCategory == null)
                return BadRequest(ModelState);
            if (categoryId != updateCategory.Id)
                return BadRequest(ModelState);
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var categoryMap = _mapper.Map<Category>(updateCategory);
            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "errow");
                return StatusCode(500, ModelState);
            }    
            return NoContent();
        }
        [HttpPost]
        public IActionResult Create([FromBody] CategoryDto categoryCreate) 
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);
            var category = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (category !=null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }  
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfull created");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }    
            var categoryDelete = _categoryRepository.GetCategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_categoryRepository.DeleteCategory(categoryDelete))
            {
                ModelState.AddModelError("", "Something went wrong while delete");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
    
}
