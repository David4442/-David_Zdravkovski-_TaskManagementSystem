using Exam.Helper;
using Exam.Models;
using Exam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Exam.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

        public CategoryController(ICategoryService categoryService, IMemoryCache cache)
        {
            _categoryService = categoryService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] PaginationParameters paginationParameters)
        {
            const string cacheKey = "all_categories";
            if (!_cache.TryGetValue(cacheKey, out var categories))
            {
              
                categories = await _categoryService.GetAllCategories(paginationParameters);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                
                _cache.Set(cacheKey, categories, cacheEntryOptions);
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var cacheKey = $"category_{id}";
            if (!_cache.TryGetValue(cacheKey, out var category))
            {
                // Fetch from DB
                category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    return NotFound();
                }

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                // Save data in cache
                _cache.Set(cacheKey, category, cacheEntryOptions);
            }

            return Ok(category);
        }
    
    //[Authorize(Roles = "Admin")]
    [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {


                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };

                await _categoryService.CreateCategory(category);

                // Return the full category with its auto-generated Id
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Categories_Name") == true)
            {
                return Conflict(new { message = "A category with this name already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                await _categoryService.UpdateCategory(id, categoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
