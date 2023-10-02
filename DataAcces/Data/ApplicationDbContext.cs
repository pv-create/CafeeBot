using DataAcces.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcces.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MathTask> MathTasks { get; set; }
    public DbSet<TelegramUser> Users { get; set; }
}

