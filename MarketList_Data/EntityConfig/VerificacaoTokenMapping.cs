using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data.EntityConfig
{
    public class VerificacaoTokenMapping : IEntityTypeConfiguration<VerificacaoToken>
    {
        public void Configure(EntityTypeBuilder<VerificacaoToken> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.NIdUsuario).IsRequired();

            builder.Property(u => u.NIdTipo).IsRequired();

            builder.Property(u => u.Token).IsRequired().HasMaxLength(200);

            builder.Property(u => u.BAtivo).HasDefaultValue(true).IsRequired();

            builder.Property(u => u.DCadastro).IsRequired().HasDefaultValueSql("now()");

            builder.HasOne(u => u.Tipo).WithMany(c => c.Tokens).HasForeignKey(u => u.NIdTipo).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Usuario).WithMany(c => c.Tokens).HasForeignKey(u => u.NIdUsuario).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("VerificacaoToken");
        }
    }
}