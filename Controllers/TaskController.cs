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
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

        public TaskController(ITaskService taskService, IMemoryCache cache)
        {
            _taskService = taskService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] PaginationParameters paginationParameters)
        {
            const string cacheKey = "all_tasks";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TaskItem> tasks))
            {
              
                tasks = await _taskService.GetAllTasks(paginationParameters);

                
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

               
                _cache.Set(cacheKey, tasks, cacheEntryOptions);
            }

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var cacheKey = $"task_{id}";
            if (!_cache.TryGetValue(cacheKey, out TaskItem task))
            {
                
                task = await _taskService.GetTaskById(id);

                if (task == null)
                {
                    return NotFound();
                }

              
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                
                _cache.Set(cacheKey, task, cacheEntryOptions);
            }

            return Ok(task);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            try
            {
                var task = new TaskItem
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    DueDate = taskDto.DueDate,
                    CategoryId = taskDto.CategoryId
                };

                await _taskService.CreateTask(task);

                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Tasks_Title") == true)
            {
                return Conflict(new { message = "A task with this title already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskDto)
        {
            try
            {
                await _taskService.UpdateTask(id, taskDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
