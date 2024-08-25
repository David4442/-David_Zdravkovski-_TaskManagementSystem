using Exam.Helper;
using Exam.Models;
using Exam.Repositories;

namespace Exam.Services
{
    public class CategoryService :ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(PaginationParameters paginationParameters)
        {
            return await _categoryRepository.GetAllCategories(paginationParameters);
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task CreateCategory(Category category)
        {
            await _categoryRepository.AddCategory(category);
        }


        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _categoryRepository.UpdateCategory(category);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            await _categoryRepository.DeleteCategory(category);
        }
    }
}
