using Microsoft.EntityFrameworkCore;
using Roxana.Application.Data.Models;

namespace Roxana.Application.Data.Contextes;

public class AuthorizeDbContext : DbContext
{
    public AuthorizeDbContext(DbContextOptions<AuthorizeDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> Tokens { get; set; }
}