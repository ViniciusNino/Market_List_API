using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class ListaAgrupadorMapping : IEntityTypeConfiguration<ListaAgrupador>
    {
        public void Configure(EntityTypeBuilder<ListaAgrupador> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.NIdAgrupadorListas).IsRequired();

            builder.Property(u => u.NIdLista).IsRequired();

            builder.HasOne(u => u.Agrupador).WithMany(c => c.ListaAgrupador).HasForeignKey(u => u.NIdAgrupadorListas).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Lista).WithMany(c => c.ListaAgrupador).HasForeignKey(u => u.NIdLista).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("ListaAgrupador");
        }
    }
}