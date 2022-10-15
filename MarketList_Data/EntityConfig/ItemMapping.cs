using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SNome).IsRequired().HasMaxLength(50);

            builder.Property(u => u.SUnidadeMedida).IsRequired().HasMaxLength(3);

            builder.Property(u => u.NIdSessao).IsRequired();

            builder.Property(u => u.NIdUnidade).IsRequired();

            builder.HasOne(u => u.Sessao).WithMany(c => c.Itens).HasForeignKey(u => u.NIdSessao).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Unidade).WithMany(c => c.Itens).HasForeignKey(u => u.NIdUnidade).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Item");
        }
    }
}