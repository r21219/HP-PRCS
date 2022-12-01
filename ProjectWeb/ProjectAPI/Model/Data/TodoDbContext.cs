using Microsoft.EntityFrameworkCore;

namespace ProjectAPI.Model.Data
{
    public class TodoDbContext:DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
