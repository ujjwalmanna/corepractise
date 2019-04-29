using Microsoft.EntityFrameworkCore;

namespace DemoAuthorizationServer.Entities
{
    public class EFUserContext : DbContext
    {
        public EFUserContext(DbContextOptions<EFUserContext> options)
           : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
    }
}
