using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VintageStore.Domain;

namespace VintageStore.Persistence.Configurations
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder
                .Property(p => p.Nombre)
                .HasMaxLength(100);

            builder
                .Property(p => p.Description)
                .HasMaxLength(500);

            builder
                .Property(p => p.DateEvent) // datetime2 se usa por default 
                .HasColumnType("datetime");

            builder
                .Property(p => p.ImageUrl)
                .IsUnicode(false)
                .HasMaxLength(1000);

            builder
                .Property(p => p.UnitPrice)
                .HasPrecision(11, 2);
        }
    }
}
