using Microsoft.EntityFrameworkCore;
using Roxana.Application.Data.Models;

namespace Roxana.Application.Data.Contextes;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> Tokens { get; set; }
}