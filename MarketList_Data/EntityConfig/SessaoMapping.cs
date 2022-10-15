using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class SessaoMapping : IEntityTypeConfiguration<Sessao>
    {
        public void Configure(EntityTypeBuilder<Sessao> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SNome).IsRequired().HasMaxLength(100);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Sessao");
        }
    }
}