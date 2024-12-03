using OrganizaMed.Infra.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using OrganizaMed.Dominio.Atividades;
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
            dbContext.Atividades.RemoveRange(dbContext.Atividades);
            dbContext.SaveChanges();

            repositorio = new RepositorioMedicos(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Medico>(repositorio.Adicionar);
        }

        [TestMethod]
        public void DeveInserirMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico.Nome, medicoSelecionado.Nome);
        }

        [TestMethod]
        public void DeveAtualizarMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            medico.Nome = "Teste de Edição";
            repositorio.Atualizar(medico);

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            Assert.IsNotNull(medicoSelecionado);
            Assert.AreEqual(medico.Nome, medicoSelecionado.Nome);
        }

        [TestMethod]
        public void DeveExcluirMedico()
        {
            var medico = Builder<Medico>
                .CreateNew()
                .With(m => m.Id = 0) // Garantir que o Id não seja configurado explicitamente
                .Persist();

            repositorio.Remover(medico);

            var medicoSelecionado = repositorio.ObterPorId(medico.Id);

            var medicos = repositorio.ObterTodos();

            Assert.IsNull(medicoSelecionado);
            Assert.AreEqual(0, medicos.Count);
        }

[TestMethod]
public void DeveAtualizarERetornarRanking()
{
    // Arrange - Criar médicos com CRMs no formato correto
    var medico1 = new Medico("João Silva", "12345-SP", "Cardiologia");
    var medico2 = new Medico("Maria Souza", "54321-SP", "Pediatria");
    var medico3 = new Medico("Carlos Oliveira", "98765-SP", "Dermatologia");

    // Criar atividades com durações diferentes para cada médico, sem sobreposição
    // Médico 1: 4 horas de trabalho (2 atividades de 2 horas)
    var atividade1 = new Atividade(0, 
        DateTime.Now.AddHours(-8), // 8:00 - 10:00
        DateTime.Now.AddHours(-6), 
        new List<Medico> { medico1 }, 
        TipoAtividade.Consulta);

    var atividade2 = new Atividade(0, 
        DateTime.Now.AddHours(-5), // 11:00 - 13:00
        DateTime.Now.AddHours(-3), 
        new List<Medico> { medico1 }, 
        TipoAtividade.Consulta);

    // Médico 2: 3 horas de trabalho (1 atividade de 3 horas)
    var atividade3 = new Atividade(0, 
        DateTime.Now.AddHours(-3), // 13:00 - 16:00
        DateTime.Now, 
        new List<Medico> { medico2 }, 
        TipoAtividade.Cirurgia);

    // Médico 3: 2 horas de trabalho (1 atividade de 2 horas)
    var atividade4 = new Atividade(0, 
        DateTime.Now.AddHours(-2), // 14:00 - 16:00
        DateTime.Now, 
        new List<Medico> { medico3 }, 
        TipoAtividade.Consulta);

    // Adicionar atividades aos médicos
    medico1.AdicionarAtividade(atividade1);
    medico1.AdicionarAtividade(atividade2);
    medico2.AdicionarAtividade(atividade3);
    medico3.AdicionarAtividade(atividade4);

    // Persistir médicos e atividades no banco
    dbContext.Medicos.AddRange(medico1, medico2, medico3);
    dbContext.Atividades.AddRange(atividade1, atividade2, atividade3, atividade4);
    dbContext.SaveChanges();

    // Act - Obter lista de médicos e atualizar ranking
    var medicos = repositorio.ObterTodos();
    medico1.AtualizarRanking(medicos);

    // Assert - Verificar se os rankings estão corretos
    var medicoMaisHoras = medicos.First(m => m.Nome == "João Silva");      // 4 horas
    var medicoHorasMedias = medicos.First(m => m.Nome == "Maria Souza");   // 3 horas
    var medicoMenosHoras = medicos.First(m => m.Nome == "Carlos Oliveira");// 2 horas

    Assert.AreEqual(1, medicoMaisHoras.Ranking);    // Deve ser 1º lugar (4 horas)
    Assert.AreEqual(2, medicoHorasMedias.Ranking);  // Deve ser 2º lugar (3 horas)
    Assert.AreEqual(3, medicoMenosHoras.Ranking);   // Deve ser 3º lugar (2 horas)

    // Verificar se as horas trabalhadas foram calculadas corretamente, com margem de tolerância
    Assert.IsTrue(Math.Abs(medicoMaisHoras.HorasTrabalhadas - 4) < 0.01, 
        $"Horas trabalhadas do médico 1 deveria ser aproximadamente 4, mas foi {medicoMaisHoras.HorasTrabalhadas}");
    Assert.IsTrue(Math.Abs(medicoHorasMedias.HorasTrabalhadas - 3) < 0.01,
        $"Horas trabalhadas do médico 2 deveria ser aproximadamente 3, mas foi {medicoHorasMedias.HorasTrabalhadas}");
    Assert.IsTrue(Math.Abs(medicoMenosHoras.HorasTrabalhadas - 2) < 0.01,
        $"Horas trabalhadas do médico 3 deveria ser aproximadamente 2, mas foi {medicoMenosHoras.HorasTrabalhadas}");
}

        [TestMethod]
        public void DeveObterAtividadesPorMedico()
        {
            var medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade1 = new Atividade(0, DateTime.Now.AddHours(-6), DateTime.Now.AddHours(-4), new List<Medico> { medico1 }, TipoAtividade.Consulta);
            var atividade2 = new Atividade(0, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico1 }, TipoAtividade.Cirurgia);

            dbContext.Medicos.Add(medico1);
            dbContext.Atividades.AddRange(atividade1, atividade2);
            dbContext.SaveChanges();

            var atividades = repositorio.ObterAtividadesPorMedico(medico1.Id);

            Assert.AreEqual(2, atividades.Count);
        }

        [TestMethod]
        public void DeveObterMedicosEnvolvidos()
        {
            var medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            var medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            var atividade = new Atividade(0, DateTime.Now.AddHours(-6), DateTime.Now.AddHours(-4), new List<Medico> { medico1, medico2 }, TipoAtividade.Consulta);
            dbContext.Atividades.Add(atividade);
            dbContext.SaveChanges();

            var medicosEnvolvidos = repositorio.ObterMedicosEnvolvidos(atividade.Id);

            Assert.AreEqual(2, medicosEnvolvidos.Count);
            Assert.IsTrue(medicosEnvolvidos.Any(m => m.Id == medico1.Id));
            Assert.IsTrue(medicosEnvolvidos.Any(m => m.Id == medico2.Id));
        }

        [TestMethod]
        public void DeveObterTodosMedicos()
        {
            var medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            var medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            repositorio.Adicionar(medico1);
            repositorio.Adicionar(medico2);

            var medicos = repositorio.ObterTodos();

            Assert.AreEqual(2, medicos.Count);
        }
    }
}