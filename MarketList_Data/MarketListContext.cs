using MarketList_API.Data;
using MarketList_Model;
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
        public virtual DbSet<Agrupador> AgrupadorListas { get; set; }
        public virtual DbSet<ListaAgrupador> ListaAgrupadorListas { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemLista> ItemLista { get; set; }
        public virtual DbSet<Lista> Lista { get; set; }
        public virtual DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual DbSet<Sessao> Sessao { get; set; }
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgrupadorMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AreaMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemListaMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListaMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListaAgrupadorMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PerfilUsuarioMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SessaoMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatusMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnidadeMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioMapping).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioUnidadeMapping).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}