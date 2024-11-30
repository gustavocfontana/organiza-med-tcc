using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Compartilhado;

namespace OrganizaMed.Infra.Atividades
{
    public class RepositorioAtividades : RepositorioBase<Atividade>, IRepositorioAtividades
    {
        public RepositorioAtividades(OrganizaMedDbContext dbContext) : base(dbContext)
        {
        }

        protected override DbSet<Atividade> ObterRegistros()
        {
            return dbContext.Atividades;
        }

        public List<Medico> ObterMedicosEnvolvidos(int atividadeId)
        {
            var atividade = dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .FirstOrDefault(a => a.Id == atividadeId);

            return atividade?.MedicosEnvolvidos;
        }
    }
}
