using Microsoft.EntityFrameworkCore;
using student_project.Models;

namespace student_project.Data
{
    public class StudentDb : DbContext
    {
        public StudentDb(DbContextOptions<StudentDb> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}
