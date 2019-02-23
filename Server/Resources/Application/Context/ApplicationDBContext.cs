using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Server.Resources.Application.Models;
using Server.Resources.Shared.Models;

namespace Server.Resources.Application.Context
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
    }
}