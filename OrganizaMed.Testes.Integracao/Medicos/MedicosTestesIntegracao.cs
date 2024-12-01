using FizzWare.NBuilder;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Compartilhado;
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
            dbContext.Atividades.RemoveRange(dbContext.Atividades);
            dbContext.SaveChanges();

            repositorio = new RepositorioMedicos(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Medico>(repositorio.Adicionar);
        }

        [TestMethod]
        public void DeveInserirMedico()
        {
            Medico ? medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            Medico ? medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico.Nome, medicoSelecionado.Nome);
        }

        [TestMethod]
        public void DeveAtualizarMedico()
        {
            Medico ? medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            medico.Nome = "Teste de Edição";
            repositorio.Atualizar(medico);

            Medico ? medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico.Nome, medicoSelecionado.Nome);
        }

        [TestMethod]
        public void DeveExcluirMedico()
        {
            Medico ? medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            repositorio.Remover(medico);

            Medico ? medicoSelecionado = repositorio.ObterPorId(medico.Id);

            List<Medico> ? medicos = repositorio.ObterTodos();

            Assert.IsNull(medicoSelecionado);
            Assert.AreEqual(0, medicos.Count);
        }

        [TestMethod]
        public void DeveAtualizarERetornarRanking()
        {
            // Arrange
            DateTime baseTime = DateTime.Now;

            Medico medico1 = new Medico("João Silva", "12345-SP", "Cardiologia");
            Medico medico2 = new Medico("Maria Santos", "67890-SP", "Cirurgia Geral");
            Medico medico3 = new Medico("Carlos Oliveira", "11111-SP", "Dermatologia");


            Atividade atividade1 = new Atividade(
                0,
                baseTime.AddHours(-4),
                baseTime.AddHours(-2),
                new List<Medico> { medico1 },
                TipoAtividade.Consulta
            );

            Atividade atividade2 = new Atividade(
                0,
                baseTime.AddHours(-6),
                baseTime.AddHours(-3),
                new List<Medico> { medico2 },
                TipoAtividade.Cirurgia
            );

            Atividade atividade3 = new Atividade(
                0,
                baseTime.AddHours(-2),
                baseTime.AddHours(-1),
                new List<Medico> { medico3 },
                TipoAtividade.Consulta
            );

            medico1.AdicionarAtividade(atividade1);
            medico2.AdicionarAtividade(atividade2);
            medico3.AdicionarAtividade(atividade3);

            repositorio.Adicionar(medico1);
            repositorio.Adicionar(medico2);
            repositorio.Adicionar(medico3);

            // Act
            var medicos = repositorio.ObterTodos().ToList();
            MedicosServico servicoMedicos = new MedicosServico(repositorio);
            servicoMedicos.AtualizarRanking();

            // Assert
            Assert.AreEqual(1, medicos.First(m => m.Nome.Contains("Maria")).Ranking);

            Assert.AreEqual(2, medicos.First(m => m.Nome.Contains("João")).Ranking);

            Assert.AreEqual(3, medicos.First(m => m.Nome.Contains("Carlos")).Ranking);

            Medico maria = medicos.First(m => m.Nome.Contains("Maria"));
            Medico joao = medicos.First(m => m.Nome.Contains("João"));
            Medico carlos = medicos.First(m => m.Nome.Contains("Carlos"));

            Assert.IsTrue(maria.HorasTrabalhadas > joao.HorasTrabalhadas);
            Assert.IsTrue(joao.HorasTrabalhadas > carlos.HorasTrabalhadas);
        }

        [TestMethod]
        public void DeveObterAtividadesPorMedico()
        {
            Medico ? medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            Atividade ? atividade1 = new Atividade(0, DateTime.Now.AddHours(-6), DateTime.Now.AddHours(-4), new List<Medico> { medico1 }, TipoAtividade.Consulta);
            Atividade ? atividade2 = new Atividade(0, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico1 }, TipoAtividade.Cirurgia);

            dbContext.Medicos.Add(medico1);
            dbContext.Atividades.AddRange(atividade1, atividade2);
            dbContext.SaveChanges();

            List<Atividade> ? atividades = repositorio.ObterAtividadesPorMedico(medico1.Id);

            Assert.AreEqual(2, atividades.Count);
        }

        [TestMethod]
        public void DeveObterMedicosEnvolvidos()
        {
            Medico ? medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            Medico ? medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            Atividade ? atividade = new Atividade(0, DateTime.Now.AddHours(-6), DateTime.Now.AddHours(-4), new List<Medico> { medico1, medico2 }, TipoAtividade.Consulta);
            dbContext.Atividades.Add(atividade);
            dbContext.SaveChanges();

            List<Medico> ? medicosEnvolvidos = repositorio.ObterMedicosEnvolvidos(atividade.Id);

            Assert.AreEqual(2, medicosEnvolvidos.Count);
            Assert.IsTrue(medicosEnvolvidos.Any(m => m.Id == medico1.Id));
            Assert.IsTrue(medicosEnvolvidos.Any(m => m.Id == medico2.Id));
        }

        [TestMethod]
        public void DeveObterTodosMedicos()
        {
            Medico ? medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            Medico ? medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            repositorio.Adicionar(medico1);
            repositorio.Adicionar(medico2);

            List<Medico> ? medicos = repositorio.ObterTodos();

            Assert.AreEqual(2, medicos.Count);
        }
    }
}
