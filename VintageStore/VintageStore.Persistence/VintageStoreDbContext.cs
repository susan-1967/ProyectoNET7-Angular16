using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;

namespace VintageStore.Persistence
{
    public class VintageStoreDbContext : IdentityDbContext<VintageStoreUserIdentity>
    {
        public VintageStoreDbContext(DbContextOptions<VintageStoreDbContext> options)
        :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FLUENT API
            // FLUENT API
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Genre>()
            //    .Property(p => p.Name)
            //    .HasMaxLength(50);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // AspNetUsers
            modelBuilder.Entity<VintageStoreUserIdentity>(e =>
            {
                e.ToTable("Usuario");
            });

            // AspNetRoles
            modelBuilder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Rol");
            });

            // AspNetUserRoles
            modelBuilder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UsuarioRol");
            });
        }
    }
}
