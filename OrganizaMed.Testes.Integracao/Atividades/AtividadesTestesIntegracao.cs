using OrganizaMed.Infra.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Atividades;
using FluentResults;
using OrganizaMed.Aplicacao.Servicos;

namespace OrganizaMed.Testes.Integracao.Atividades
{
    [TestClass]
    [TestCategory("Integracao")]
    public class AtividadesTestesIntegracao
    {
        private OrganizaMedDbContext dbContext;
        private RepositorioAtividades repositorio;
        private AtividadesServico servico;

        [TestInitialize]
        public void Inicializar()
        {
            dbContext = new OrganizaMedDbContext();

            dbContext.Atividades.RemoveRange(dbContext.Atividades);

            repositorio = new RepositorioAtividades(dbContext);
            servico = new AtividadesServico(repositorio);

            BuilderSetup.SetCreatePersistenceMethod<Atividade>(repositorio.Adicionar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Atividade>>(atividades =>
            {
                foreach (var atividade in atividades)
                {
                    repositorio.Adicionar(atividade);
                }
            });
        }

        [TestMethod]
        public void DeveInserirAtividade()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            var resultado = servico.Adicionar(atividade);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(resultado.Value);
        }

        [TestMethod]
        public void DeveAtualizarAtividade()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            atividade.DataFim = DateTime.Now.AddHours(2);
            var resultado = servico.Atualizar(atividade);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(atividade.DataFim, resultado.Value.DataFim);
        }

        [TestMethod]
        public void DeveExcluirAtividade()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            var resultado = servico.Remover(atividade.Id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNull(repositorio.ObterPorId(atividade.Id));
        }

        [TestMethod]
        public void DeveObterAtividadePorId()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            var resultado = servico.ObterPorId(atividade.Id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(resultado.Value);
        }

        [TestMethod]
        public void DeveObterTodasAtividades()
        {
            var atividades = Builder<Atividade>
                .CreateListOfSize(5)
                .All()
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            var resultado = servico.ObterTodos();

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(5, resultado.Value.Count);
        }
    }
}
