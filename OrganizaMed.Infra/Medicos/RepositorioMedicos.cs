using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;
using OrganizaMed.Infra.Compartilhado;

namespace OrganizaMed.Infra.Medicos
{
    public class RepositorioMedicos : RepositorioBase<Medico>, IRepositorioMedicos
    {

        public RepositorioMedicos(OrganizaMedDbContext dbContext) : base(dbContext)
        {
        }

        public List<Medico> ObterTodos()
        {
            return dbContext.Medicos
                .Include(m => m.Atividades)
                .ToList();
        }

        protected override DbSet<Medico> ObterRegistros()
        {
            return dbContext.Medicos;
        }

        public List<Medico> ObterMedicosEnvolvidos(int atividadeId)
        {
            var atividade = dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .FirstOrDefault(a => a.Id == atividadeId);

            return atividade?.MedicosEnvolvidos;
        }

        public List<Atividade> ObterAtividadesPorMedico(int medicoId)
        {
            return dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .Where(a => a.MedicosEnvolvidos.Any(m => m.Id == medicoId))
                .ToList();
        }
    }
}
