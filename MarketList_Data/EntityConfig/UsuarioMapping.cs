using MarketList_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketList_Data
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.SUsuario).HasMaxLength(100);

            builder.Property(u => u.SEmail).IsRequired().HasMaxLength(100);

            builder.HasIndex(u => u.SEmail).IsUnique();

            builder.Property(u => u.SSenha).IsRequired().HasMaxLength(500);

            builder.Property(u => u.SNome).IsRequired().HasMaxLength(150);

            builder.Property(u => u.NIdPerfilUsuario).IsRequired();

            builder.Property(u => u.NIdStatus).HasDefaultValue((int)StatusUsuarioEnum.Aguardando_Ativacao_Email).IsRequired();

            builder.Property(u => u.DCadastro).HasDefaultValueSql("now()").IsRequired();

            builder.HasOne(u => u.PerfilUsuario).WithMany(c => c.Usuarios).HasForeignKey(u => u.NIdPerfilUsuario).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Status).WithMany(c => c.Usuarios).HasForeignKey(u => u.NIdStatus).OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(u => u.ValidationResult);
            builder.Ignore(u => u.CascadeMode);

            builder.ToTable("Usuario");
        }
    }
}