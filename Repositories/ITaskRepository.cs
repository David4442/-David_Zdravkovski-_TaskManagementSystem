using Exam.Helper;
using Exam.Models;

namespace Exam.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasks(PaginationParameters paginationParameters);
        Task<TaskItem> GetTaskById(int id);
        Task AddTask(TaskItem task);
        Task UpdateTask(TaskItem task);
        Task DeleteTask(TaskItem task);
    }
}
