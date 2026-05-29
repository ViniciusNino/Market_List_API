using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class SetorMapping : IEntityTypeConfiguration<Setor>
    {
        public void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SNome).IsRequired().HasMaxLength(50);

            builder.Property(u => u.NIdSessao).IsRequired();

            builder.HasOne(u => u.Sessao)
                .WithMany(c => c.Setores)
                .HasForeignKey(u => u.NIdSessao)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.ClassLevelCascadeMode);
            builder.Ignore(u => u.RuleLevelCascadeMode);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Setor");
        }
    }
}
