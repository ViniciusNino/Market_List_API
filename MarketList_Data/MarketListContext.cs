using MarketList_Model;
using MedPlannerCore.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Data
{
    public partial class MarketListContext : DbContext
    {
        public MarketListContext()
        {
        }

        public MarketListContext(DbContextOptions<MarketListContext> options) : base(options)
        {
        }
        public virtual DbSet<AgrupadorListas> AgrupadorListas { get; set; }
        public virtual DbSet<ListaAgrupadorListas> ListaAgrupadorListas { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemLista> ItemLista { get; set; }
        public virtual DbSet<Lista> Lista { get; set; }
        public virtual DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual DbSet<Sessao> Sessao { get; set; }
        public virtual DbSet<StatusItemLista> StatusItemLista { get; set; }
        public virtual DbSet<StatusUsuario> StatusUsuario { get; set; }
        public virtual DbSet<Unidade> Unidade { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioUnidade> UsuarioUnidade { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(Common.GetSettings("DefaultConnectionPGSQL")).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgrupadorListas>().HasOne(x => x.Status).WithMany().HasForeignKey(c => c.Status);
            modelBuilder.Entity<AgrupadorListas>().HasOne(x => x.Usuario).WithMany().HasForeignKey(c => c.NIdUsuario);
            modelBuilder.Entity<AgrupadorListas>().Property(p => p.DCadastro).HasDefaultValueSql("now()");


        }
    }
}