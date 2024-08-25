using Exam.Data;
using Exam.Helper;
using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TaskManagementContext _context;

        public CategoryRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(PaginationParameters paginationParameters)
        {
            var query = _context.Categories.AsQueryable();

            // Apply sorting
            if (!string.IsNullOrEmpty(paginationParameters.SortBy))
            {
                query = paginationParameters.SortDescending
                    ? query.OrderByDescending(c => EF.Property<object>(c, paginationParameters.SortBy))
                    : query.OrderBy(c => EF.Property<object>(c, paginationParameters.SortBy));
            }

            // Apply pagination
            return await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
