using Exam.Helper;
using Exam.Models;
using Exam.Repositories;

namespace Exam.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks(PaginationParameters paginationParameters)
        {
            return await _taskRepository.GetAllTasks(paginationParameters);
        }

        public async Task<TaskItem> GetTaskById(int id)
        {
            return await _taskRepository.GetTaskById(id);
        }

        public async Task CreateTask(TaskItem task)
        {
            await _taskRepository.AddTask(task);
        }

        public async Task UpdateTask(int id, TaskDto taskDto)
        {
            var task = await _taskRepository.GetTaskById(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.CategoryId = taskDto.CategoryId;

            await _taskRepository.UpdateTask(task);
        }

        public async Task DeleteTask(int id)
        {
            var task = await _taskRepository.GetTaskById(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            await _taskRepository.DeleteTask(task);
        }
    }
}
