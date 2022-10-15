using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class ItemListaMapping : IEntityTypeConfiguration<ItemLista>
    {
        public void Configure(EntityTypeBuilder<ItemLista> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.NIdItem).IsRequired();

            builder.Property(u => u.NIdLista).IsRequired();

            builder.Property(u => u.NIdStatus).HasDefaultValue((int)StatusItemListaEnum.Solicitado).IsRequired();

            builder.Property(u => u.NIdUsuarioComprador).IsRequired();

            builder.Property(u => u.NIdUsuarioSolicitante).IsRequired();

            builder.Property(u => u.NQuantidade).IsRequired();

            builder.Property(u => u.DCadastro).HasDefaultValueSql("now()").IsRequired();

            builder.Property(u => u.BAtivo).HasDefaultValue(true).IsRequired();

            builder.HasOne(u => u.Item).WithMany(c => c.ItensLista).HasForeignKey(u => u.NIdItem).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Lista).WithMany(c => c.ItensLista).HasForeignKey(u => u.NIdLista).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Status).WithMany(c => c.ItensLista).HasForeignKey(u => u.NIdStatus).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.UsuarioComprador).WithMany(c => c.ItensListaComprador).HasForeignKey(u => u.NIdUsuarioComprador).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.UsuarioSolicitante).WithMany(c => c.ItensListaSolicitante).HasForeignKey(u => u.NIdUsuarioSolicitante).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("ItemLista");
        }
    }
}