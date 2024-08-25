using Exam.Helper;
using Exam.Models;

namespace Exam.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories(PaginationParameters paginationParameters);
        Task<Category> GetCategoryById(int id);
        Task CreateCategory(Category category);
        Task UpdateCategory(int id, CategoryDto categoryDto);
        Task DeleteCategory(int id);
    }
}
