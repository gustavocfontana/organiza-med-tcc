using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Compartilhado;

namespace OrganizaMed.Infra.Atividades
{
    public class RepositorioAtividades : IRepositorioAtividades
    {
        readonly private OrganizaMedDbContext dbContext;

        public RepositorioAtividades(OrganizaMedDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Adicionar(Atividade atividade)
        {
            dbContext.Atividades.Add(atividade);
            dbContext.SaveChanges();
        }

        public void Atualizar(Atividade atividade)
        {
            dbContext.Atividades.Update(atividade);
            dbContext.SaveChanges();
        }

        public Atividade ObterPorId(int id)
        {
            return dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<Atividade> ObterTodos()
        {
            return dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .ToList();
        }

        public void Remover(Atividade atividade)
        {
            dbContext.Atividades.Remove(atividade);
            dbContext.SaveChanges();
        }

        public List<Medico> ObterMedicosEnvolvidos(int atividadeId)
        {
            Atividade ? atividade = dbContext.Atividades
                .Include(a => a.MedicosEnvolvidos)
                .FirstOrDefault(a => a.Id == atividadeId);

            return atividade?.MedicosEnvolvidos ?? new List<Medico>();
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
