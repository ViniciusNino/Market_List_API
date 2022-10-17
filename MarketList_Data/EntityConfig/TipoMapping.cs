using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class TipoMapping : IEntityTypeConfiguration<Tipo>
    {
        public void Configure(EntityTypeBuilder<Tipo> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SDescricao).IsRequired().HasMaxLength(100);

            builder.Property(u => u.NIdArea).IsRequired();

            builder.HasOne(u => u.Area).WithMany(c => c.Tipos).HasForeignKey(u => u.NIdArea).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Tipo");
        }
    }
}