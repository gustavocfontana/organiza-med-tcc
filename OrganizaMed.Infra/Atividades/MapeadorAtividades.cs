using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizaMed.Dominio.Atividades;

namespace OrganizaMed.Infra.Atividades
{
    public class MapeadorAtividades : IEntityTypeConfiguration<Atividade>
    {
        public void Configure(EntityTypeBuilder<Atividade> builder)
        {
            builder.ToTable("TBAtividades");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.DataInicio)
                .IsRequired();

            builder.Property(a => a.DataFim)
                .IsRequired();

            builder.Property(a => a.TipoAtividade)
                .IsRequired();

            builder.HasMany(a => a.MedicosEnvolvidos)
                .WithMany(m => m.Atividades)
                .UsingEntity(j => j.ToTable("AtividadeMedicos"));
        }
    }
}
