using Microsoft.EntityFrameworkCore;
using ToDoAspNetCore.Models;

namespace ToDoAspNetCore.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoItemModel> ToDoItems { get; set; }
    }
}
