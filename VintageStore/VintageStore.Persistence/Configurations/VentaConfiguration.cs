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
    public  class VentaConfiguration : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.Property(p => p.NumeroOperacion)
                .IsUnicode(false)
                .HasMaxLength(20);

            builder.Property(p => p.Total)
                .HasPrecision(11, 2);

            builder.Property(p => p.FechaVenta)
                .HasColumnType("date")
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
