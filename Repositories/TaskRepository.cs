using Exam.Data;
using Exam.Helper;
using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks(PaginationParameters paginationParameters)
        {
            var query = _context.Tasks.AsQueryable();

            // Apply sorting
            if (!string.IsNullOrEmpty(paginationParameters.SortBy))
            {
                query = paginationParameters.SortDescending
                    ? query.OrderByDescending(t => EF.Property<object>(t, paginationParameters.SortBy))
                    : query.OrderBy(t => EF.Property<object>(t, paginationParameters.SortBy));
            }

            // Apply pagination
            return await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskById(int id)
        {
            return await _context.Tasks.Include(t => t.Category).SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTask(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
