using Microsoft.EntityFrameworkCore;

namespace Server.Resources.Identity.Context
{
    public class IdentityDBContext : DbContext
    {
        public IdentityDBContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}