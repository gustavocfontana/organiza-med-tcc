
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Autenticacao;
using OrganizaMed.Dominio.Medicos;
using OrganizaMed.Infra.Atividades;
using OrganizaMed.Infra.Medicos;

namespace OrganizaMed.Infra.Compartilhado
{
    public class OrganizaMedDbContext : IdentityDbContext<Usuario, Perfil, int>
    {
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("SqlServer");

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