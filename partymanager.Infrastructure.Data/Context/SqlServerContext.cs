using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using partymanager.Domain.Entidades;
using partymanager.Infrastructure.Data.Mapping;

namespace partymanager.Infrastructure.Data.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        // DbSet para cada entidade do seu sistema
        public DbSet<TipoItem> tipoitem { get; set; }
        public DbSet<TipoEvento> tipoevento { get; set; }

        public DbSet<Item> item { get; set; }
        public DbSet<TipoEventoItem> tipoeventoitem { get; set; }
        public DbSet<Evento> evento { get; set; }
        public DbSet<ItemCalculado> itemcalculado { get; set; }
        public DbSet<Usuario> usuario { get; set; }


        // Configuração da string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = @"Server=sql5106.site4now.net;Database=db_aa9649_partymanagerdb;User Id=db_aa9649_partymanagerdb_admin;Password=Gu_448228;TrustServerCertificate=True;";
                // var stringConexao = @"Server=GUSTACNOTE;DataBase=PartyManagerv60;integrated security=true;TrustServerCertificate=True;";

                optionsBuilder.UseSqlServer(stringConexao);
            }
        }

        // Configurações de Mappingeamento de entidade
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar Mappingeamento de entidades
            modelBuilder.ApplyConfiguration(new TipoItemMapping());
            modelBuilder.ApplyConfiguration(new TipoEventoMapping());
            modelBuilder.ApplyConfiguration(new ItemMapping());
            modelBuilder.ApplyConfiguration(new TipoEventoItemMapping());
            modelBuilder.ApplyConfiguration(new EventoMapping());
            modelBuilder.ApplyConfiguration(new ItemCalculadoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());

        }
    }

    // Fábrica para permitir a criação de DbContext em tempo de design
    public class SqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
    {
        public SqlServerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerContext>();
            //var stringConexao = @"Server=GUSTACNOTE;DataBase=PartyManagerv60;integrated security=true;TrustServerCertificate=True;";
            var stringConexao = @"Server=sql5106.site4now.net;Database=db_aa9649_partymanagerdb;User Id=db_aa9649_partymanagerdb_admin;Password=Gu_448228;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(stringConexao);

            return new SqlServerContext(optionsBuilder.Options);
        }
    }
}
