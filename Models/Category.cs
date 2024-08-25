using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace Exam.Models
{
    [Index(nameof(Name), IsUnique = true)]

    public class Category
    {
        public int Id { get; set; }                 
        public string Name { get; set; }            
        public string Description { get; set; }

    }
}
