using Exam.Helper;
using Exam.Models;

namespace Exam.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasks(PaginationParameters paginationParameters);
        Task<TaskItem> GetTaskById(int id);
        Task CreateTask(TaskItem task);
        Task UpdateTask(int id, TaskDto taskDto);
        Task DeleteTask(int id);
    }
}
