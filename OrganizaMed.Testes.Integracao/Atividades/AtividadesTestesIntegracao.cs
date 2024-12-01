using FizzWare.NBuilder;
using FluentResults;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Atividades;
using OrganizaMed.Infra.Compartilhado;
using OrganizaMed.Infra.Medicos;

namespace OrganizaMed.Testes.Integracao.Atividades
{
    [TestClass]
    [TestCategory("Integracao")]
    public class AtividadesTestesIntegracao
    {
        private OrganizaMedDbContext dbContext;
        private RepositorioAtividades repositorioAtividades;
        private RepositorioMedicos repositorioMedicos;
        private AtividadesServico servico;

        [TestInitialize]
        public void Inicializar()
        {
            dbContext = new OrganizaMedDbContext();

            dbContext.Atividades.RemoveRange(dbContext.Atividades);
            dbContext.SaveChanges();

            repositorioAtividades = new RepositorioAtividades(dbContext);
            servico = new AtividadesServico(repositorioAtividades, repositorioMedicos);

            BuilderSetup.SetCreatePersistenceMethod<Atividade>(repositorioAtividades.Adicionar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Atividade>>(atividades =>
            {
                foreach (Atividade ? atividade in atividades)
                    repositorioAtividades.Adicionar(atividade);
            });
        }

        [TestMethod]
        public void DeveInserirAtividade()
        {
            // Arrange
            DateTime dataInicio = DateTime.Now.AddDays(1).Date.AddHours(14); // Amanhã às 14:00
            DateTime dataFim = dataInicio.AddMinutes(30); // 30 minutos de duração
            Medico medico = new Medico("Dr. Test", "12345-SP", "Cardiologia");

            // Verifica disponibilidade antes de criar a atividade
            Assert.IsTrue(medico.EstaDisponivel(dataInicio, dataFim), "O médico deveria estar disponível");

            Atividade atividade = new Atividade(
                0,
                dataInicio,
                dataFim,
                new List<Medico> { medico },
                TipoAtividade.Consulta
            );

            // Act
            medico.AdicionarAtividade(atividade);

            // Assert
            Assert.AreEqual(1, medico.Atividades.Count, "A atividade deveria ter sido adicionada");
            Assert.AreEqual(dataInicio, medico.Atividades.First().DataInicio);
            Assert.AreEqual(dataFim, medico.Atividades.First().DataFim);
        }

        [TestMethod]
        public void DeveAtualizarAtividade()
        {
            Atividade ? atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            atividade.DataFim = DateTime.Now.AddHours(2);
            Result<Atividade> ? resultado = servico.Atualizar(atividade);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(atividade.DataFim, resultado.Value.DataFim);
        }

        [TestMethod]
        public void DeveExcluirAtividade()
        {
            Atividade ? atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            Result<Atividade> ? resultado = servico.Remover(atividade.Id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNull(repositorioAtividades.ObterPorId(atividade.Id));
        }

        [TestMethod]
        public void DeveObterAtividadePorId()
        {
            Atividade ? atividade = Builder<Atividade>
                .CreateNew()
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            Result<Atividade> ? resultado = servico.ObterPorId(atividade.Id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(resultado.Value);
        }

        [TestMethod]
        public void DeveObterTodasAtividades()
        {
            IList<Atividade> ? atividades = Builder<Atividade>
                .CreateListOfSize(5)
                .All()
                .With(a => a.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .With(a => a.DataInicio = DateTime.Now)
                .With(a => a.DataFim = DateTime.Now.AddHours(1))
                .Persist();

            Result<List<Atividade>> ? resultado = servico.ObterTodos();

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(5, resultado.Value.Count);
        }

        [TestMethod]
        public void ObterMedicosEnvolvidos_DeveRetornarMedicosParaAtividade()
        {
            // Arrange
            Medico ? medico = new Medico("Dr. Test", "12345678", "Cardiologia");
            DateTime dataInicio = DateTime.Now;
            DateTime dataFim = dataInicio.AddHours(1);

            Atividade ? atividade = new Atividade(
                0,
                dataInicio,
                dataFim,
                new List<Medico> { medico },
                TipoAtividade.Consulta
            );

            repositorioAtividades.Adicionar(atividade);

            // Act
            List<Medico> ? medicosEnvolvidos = repositorioAtividades.ObterMedicosEnvolvidos(atividade.Id);

            // Assert
            Assert.IsNotNull(medicosEnvolvidos);
            Assert.IsTrue(medicosEnvolvidos.Count > 0);
            Assert.AreEqual(medico.Nome, medicosEnvolvidos[0].Nome);
            Assert.AreEqual(medico.Crm, medicosEnvolvidos[0].Crm);
        }
    }
}
