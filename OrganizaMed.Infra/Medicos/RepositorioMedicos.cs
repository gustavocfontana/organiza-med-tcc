using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Medicos;
using OrganizaMed.Infra.Compartilhado;

namespace OrganizaMed.Infra.Medicos
{
    public class RepositorioMedicos : RepositorioBase<Medico>, IRepositorioMedicos
    {

        public RepositorioMedicos(OrganizaMedDbContext dbContext) : base(dbContext)
        {
        }

        protected override DbSet<Medico> ObterRegistros()
        {
            return dbContext.Medicos;
        }
    }
}
