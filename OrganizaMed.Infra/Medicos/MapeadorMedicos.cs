using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizaMed.Dominio.Medicos;

public class MapeadorMedicos : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        builder.ToTable("TBMedicos");

        builder.Property(m => m.Id)
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(m => m.Nome)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(m => m.Crm)
            .HasColumnType("varchar(8)")
            .IsRequired();

        builder.Property(m => m.Especialidade)
            .HasColumnType("varchar(100)")
            .IsRequired();
    }
}