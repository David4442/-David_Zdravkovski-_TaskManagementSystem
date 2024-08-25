using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Exam.Models
{
    [Index(nameof(Title), IsUnique = true)]

    public class TaskItem
    {
        public int Id { get; set; }                 // Primary Key
        public string Title { get; set; }           // Title of the Task
        public string Description { get; set; }     // Description of the Task
        public DateTime DueDate { get; set; }       // Due Date of the Task
        public int CategoryId { get; set; }         // Foreign Key to Category

        // Navigation property
        public Category Category { get; set; }
    }
}
