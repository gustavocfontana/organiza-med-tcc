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
using OrganizaMed.Infra.Medicos;

namespace OrganizaMed.Testes.Integracao.Atividades
{
    [TestClass]
    [TestCategory("Integracao")]
    public class AtividadesTestesIntegracao
    {
        private OrganizaMedDbContext dbContext;
        private RepositorioAtividades repositorio;
        private AtividadesServico servico;
        private MedicosServico medicosServico;
        private RepositorioMedicos repositorioMedicos;

        [TestInitialize]
        public void Inicializar()
        {
            dbContext = new OrganizaMedDbContext();

            dbContext.Atividades.RemoveRange(dbContext.Atividades);
            dbContext.SaveChanges();

            repositorio = new RepositorioAtividades(dbContext);
            servico = new AtividadesServico(repositorio, repositorioMedicos);

            repositorioMedicos = new RepositorioMedicos(dbContext);
            medicosServico = new MedicosServico(repositorioMedicos);

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
            // Arrange
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.Id = 0)
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Build();

            // Act
            var resultado = servico.Adicionar(atividade);

            // Assert
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(resultado.Value);
            Assert.AreNotEqual(0, resultado.Value.Id);
            Assert.AreEqual(atividade.DataInicio, resultado.Value.DataInicio);
            Assert.AreEqual(atividade.DataFim, resultado.Value.DataFim);
        }

        [TestMethod]
        public void DeveAtualizarAtividade()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
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
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
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
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
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
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            var resultado = servico.ObterTodos();

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(5, resultado.Value.Count);
        }

        [TestMethod]
        public void ObterMedicosEnvolvidos_DeveRetornarMedicosParaAtividade()
        {
            // Arrange
            var medico = new Medico("Dr. Test", "12345678", "Cardiologia");
            var dataInicio = DateTime.Now;
            var dataFim = dataInicio.AddHours(1);

            var atividade = new Atividade(
                id: 0,
                dataInicio: dataInicio,
                dataFim: dataFim,
                medicosEnvolvidos: new List<Medico> { medico },
                tipoAtividade: TipoAtividade.Consulta
            );

            repositorio.Adicionar(atividade);

            // Act
            var medicosEnvolvidos = repositorio.ObterMedicosEnvolvidos(atividade.Id);

            // Assert
            Assert.IsNotNull(medicosEnvolvidos);
            Assert.IsTrue(medicosEnvolvidos.Count > 0);
            Assert.AreEqual(medico.Nome, medicosEnvolvidos[0].Nome);
            Assert.AreEqual(medico.Crm, medicosEnvolvidos[0].Crm);
        }
    }
}