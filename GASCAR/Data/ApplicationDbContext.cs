using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gascar.Models;

namespace Gascar.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Auto> Auto { get; set; }
        public DbSet<PostoAuto> PostiAuto { get; set; }
        public DbSet<Sosta> Soste { get; set; }
        public DbSet<Ricarica> Ricariche { get; set; }
        public DbSet<Pagamento> Pagamenti { get; set; }
        public DbSet<Configurazione> Configurazioni { get; set; }
    }
}
