using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class ListaMapping : IEntityTypeConfiguration<Lista>
    {
        public void Configure(EntityTypeBuilder<Lista> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.BAtivo).HasDefaultValue(true).IsRequired();

            builder.Property(u => u.DCadastro).HasDefaultValueSql("now()").IsRequired();

            builder.Property(u => u.NIdUnidade).IsRequired();

            builder.Property(u => u.NIdUsuario).IsRequired();

            builder.HasOne(u => u.Unidade).WithMany(c => c.Listas).HasForeignKey(u => u.NIdUnidade).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Usuario).WithMany(c => c.Listas).HasForeignKey(u => u.NIdUsuario).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Lista");
        }
    }
}