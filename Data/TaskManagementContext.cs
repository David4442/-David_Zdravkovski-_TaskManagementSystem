using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
