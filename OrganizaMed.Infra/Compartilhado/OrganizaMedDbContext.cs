using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Atividades;
using OrganizaMed.Infra.Medicos;

namespace OrganizaMed.Infra.Compartilhado
{
    public class OrganizaMedDbContext : DbContext
    {
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot ? config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string ? connectionString = config.GetConnectionString("SqlServer");

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MapeadorMedicos());
            modelBuilder.ApplyConfiguration(new MapeadorAtividades());

            base.OnModelCreating(modelBuilder);
        }
    }
}
