using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrganizaMed.Dominio.Compartilhado;

namespace OrganizaMed.Infra.Compartilhado
{
    public abstract class RepositorioBase<T> where T : EntidadeBase
    {
        protected readonly OrganizaMedDbContext dbContext;

        public RepositorioBase(OrganizaMedDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected abstract DbSet<T> ObterRegistros();

        public void Adicionar(T entidade)
        {
            if (entidade.Id != 0)
            {
                entidade.Id = 0;
            }

            ObterRegistros().Add(entidade);
            dbContext.SaveChanges();
        }

        public void Atualizar(T entidade)
        {
            ObterRegistros().Update(entidade);
            dbContext.SaveChanges();
        }

        public void Remover(T entidade)
        {
            ObterRegistros().Remove(entidade);
            dbContext.SaveChanges();
        }

        public virtual T ? ObterPorId(int id)
        {
            return ObterRegistros()
                .FirstOrDefault(x => x.Id == id);
        }

        public virtual List<T> ObterTodos()
        {
            return ObterRegistros()
                .ToList();
        }
    }
}
