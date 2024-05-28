using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class UnidadeMapping : IEntityTypeConfiguration<Unidade>
    {
        public void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SNome).IsRequired().HasMaxLength(100);
            
            builder.Property(u => u.DCadastro).HasDefaultValueSql("GETDATE()").IsRequired();

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.ClassLevelCascadeMode);
            builder.Ignore(u => u.RuleLevelCascadeMode);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Unidade");
        }
    }
}