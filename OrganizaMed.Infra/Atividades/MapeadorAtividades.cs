using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            builder.Property(a => a.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(a => a.Descricao)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(a => a.DataInicio)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(a => a.DataFim)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasMany(a => a.MedicosEnvolvidos)
                .WithMany()
                .UsingEntity(j => j.ToTable("TBAtividadesMedicos"));

            builder.Property(a => a.RecoveryTime)
                .HasColumnType("time")
                .IsRequired();
        }
    }
}
