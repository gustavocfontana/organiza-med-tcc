using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrganizaMed.Infra.Compartilhado
{
    public static class MigradorBancoDados
    {
        public static bool AtualizarBancoDados(DbContext dbContext)
        {
            var qtdMigracoesPendentes = dbContext.Database.GetPendingMigrations().Count();
            if (qtdMigracoesPendentes == 0)
            {
                Console.WriteLine("Nenhuma migração pendente, continuando...");
                return false;
            }
            Console.WriteLine("Aplicando migrações pendentes, isso pode demorar alguns segundos...");
            dbContext.Database.Migrate();
            return true;
        }
    }
}
