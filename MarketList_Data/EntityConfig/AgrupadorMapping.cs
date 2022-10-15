using System;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class AgrupadorMapping : IEntityTypeConfiguration<Agrupador>
    {
        public void Configure(EntityTypeBuilder<Agrupador> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SDescricao).IsRequired().HasMaxLength(100);

            builder.Property(u => u.DCadastro).HasDefaultValueSql("now()").IsRequired(); ;

            builder.Property(u => u.NIdStatus).HasDefaultValue((int)StatusAgrupadoEnum.Ativo).IsRequired();

            builder.Property(u => u.NIdUsuario).IsRequired();

            builder.HasOne(u => u.Status).WithMany(c => c.Agrupadores).HasForeignKey(u => u.NIdStatus).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Usuario).WithMany(c => c.Agrupadores).HasForeignKey(u => u.NIdUsuario).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Agrupador");
        }
    }
}