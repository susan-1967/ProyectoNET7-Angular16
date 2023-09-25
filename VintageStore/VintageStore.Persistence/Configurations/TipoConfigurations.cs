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
    public class GenreConfigurations : IEntityTypeConfiguration<Tipo>
    {
        public void Configure(EntityTypeBuilder<Tipo> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(50);

            builder
                .HasQueryFilter(p => p.Status == true);
        }
    }
}
