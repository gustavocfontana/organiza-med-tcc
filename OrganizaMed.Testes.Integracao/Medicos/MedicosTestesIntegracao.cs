using OrganizaMed.Infra.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using OrganizaMed.Dominio.Medicos;
using OrganizaMed.Infra.Medicos;

namespace OrganizaMed.Testes.Integracao.Medicos
{
    [TestClass]
    [TestCategory("Integracao")]
    public class MedicosTestesIntegracao
    {
        private OrganizaMedDbContext dbContext;
        private RepositorioMedicos repositorio;

        [TestInitialize]
        public void Inicializar()
        {
            dbContext = new OrganizaMedDbContext();

            dbContext.Medicos.RemoveRange(dbContext.Medicos);

            repositorio = new RepositorioMedicos(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Medico>(repositorio.Adicionar);
        }

        [TestMethod]
        public void DeveInserirMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .Persist();

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico, medicoSelecionado);
        }

        [TestMethod]
        public void DeveAtualizarMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .Persist();

            medico.Nome = "Teste de Edição";
            repositorio.Atualizar(medico);

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico, medicoSelecionado);
        }

        [TestMethod]
        public void DeveExcluirMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .Persist();

            repositorio.Remover(medico);

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            var medicos = repositorio.ObterTodos();

            Assert.IsNull(medicoSelecionado);
            Assert.AreEqual(0, medicos.Count);
        }
    }
}
