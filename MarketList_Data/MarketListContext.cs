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
        public virtual DbSet<AgrupadorListas> AgrupadorListas { get; set; }
        public virtual DbSet<ListaAgrupadorListas> ListaAgrupadorListas { get; set; }
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
            modelBuilder.Entity<AgrupadorListas>().HasOne(x => x.Usuario).WithMany().HasForeignKey(c => c.NIdUsuario);
            modelBuilder.Entity<AgrupadorListas>().HasOne(x => x.Status).WithMany().HasForeignKey(c => c.NIdStatus);
            modelBuilder.Entity<AgrupadorListas>().Property(p => p.DCadastro).HasDefaultValueSql("now()");

            modelBuilder.Entity<Item>().HasOne(x => x.Sessao).WithMany().HasForeignKey(c => c.NIdSessao);
            modelBuilder.Entity<Item>().HasOne(x => x.Unidade).WithMany().HasForeignKey(c => c.NIdUnidade);

            modelBuilder.Entity<ItemLista>().HasOne(x => x.Lista).WithMany().HasForeignKey(c => c.NIdLista);
            modelBuilder.Entity<ItemLista>().HasOne(x => x.Item).WithMany().HasForeignKey(c => c.NIdItem);
            modelBuilder.Entity<ItemLista>().HasOne(x => x.Status).WithMany().HasForeignKey(c => c.NIdStatus);
            modelBuilder.Entity<ItemLista>().HasOne(x => x.UsuarioComprador).WithMany().HasForeignKey(c => c.NIdUsuarioComprador);
            modelBuilder.Entity<ItemLista>().HasOne(x => x.UsuarioSolicitante).WithMany().HasForeignKey(c => c.NIdUsuarioSolicitante);
            modelBuilder.Entity<ItemLista>().Property(p => p.DCadastro).HasDefaultValueSql("now()");

            modelBuilder.Entity<Lista>().HasOne(x => x.Unidade).WithMany().HasForeignKey(c => c.NIdUnidade);
            modelBuilder.Entity<Lista>().HasOne(x => x.Usuario).WithMany().HasForeignKey(c => c.NIdUsuario);
            modelBuilder.Entity<Lista>().Property(p => p.DCadastro).HasDefaultValueSql("now()");

            modelBuilder.Entity<ListaAgrupadorListas>().HasOne(x => x.AgrupadorListas).WithMany().HasForeignKey(c => c.NIdAgrupadorListas);
            modelBuilder.Entity<ListaAgrupadorListas>().HasOne(x => x.Lista).WithMany().HasForeignKey(c => c.NIdLista);

            modelBuilder.Entity<Status>().HasOne(x => x.Area).WithMany().HasForeignKey(c => c.NIdArea);

            modelBuilder.Entity<Usuario>().HasOne(x => x.PerfilUsuario).WithMany().HasForeignKey(c => c.NIdPerfilUsuario);
            modelBuilder.Entity<Usuario>().HasOne(x => x.Status).WithMany().HasForeignKey(c => c.NIdStatus);
            modelBuilder.Entity<Usuario>().Property(p => p.DCadastro).HasDefaultValueSql("now()");
        }
    }
}