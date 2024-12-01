using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Testes.Unidade.Medicos
{
    [TestClass]
    [TestCategory("Unidade")]
    public class MedicosTestesUnidade
    {
        [TestMethod]
        public void DeveCriarValida()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            Assert.IsNotNull(medico);
            Assert.AreEqual("João Silva", medico.Nome);
            Assert.AreEqual("12345678", medico.Crm);
            Assert.AreEqual("Cardiologia", medico.Especialidade);
        }

        [TestMethod]
        public void DeveCriarErro()
        {
            var medico = new Medico("Jo", "1234", "");
            var erros = medico.Validar();
            Assert.IsTrue(erros.Contains("Nome inválido"));
            Assert.IsTrue(erros.Contains("CRM inválido"));
            Assert.IsTrue(erros.Contains("Especialidade inválida"));
        }

        [TestMethod]
        public void MedicoEstaDisponivelConsultaRecuperacao()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddMinutes(30), new List < Medico > { medico }, TipoAtividade.Consulta);
            medico.AdicionarAtividade(atividade);

            var disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2));
            Assert.IsTrue(disponibilidade);
        }

        [TestMethod]
        public void MedicoEstaDisponivelCirurgiaRecuperacao()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade = new Atividade(1,DateTime.Now, DateTime.Now.AddHours(2), new List < Medico > { medico }, TipoAtividade.Cirurgia);
            medico.AdicionarAtividade(atividade);

            var disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(5), DateTime.Now.AddHours(6));
            Assert.IsFalse(disponibilidade);
        }

        [TestMethod]
        public void DeveCalcularHorasTrabalhadasCorretamente()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade1 = new Atividade(1, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico }, TipoAtividade.Consulta);
            var atividade2 = new Atividade(2, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), new List<Medico> { medico }, TipoAtividade.Cirurgia);
            medico.AdicionarAtividade(atividade1);
            medico.AdicionarAtividade(atividade2);

            medico.CalcularHorasTrabalhadas();

            double expectedHorasTrabalhadas = 3.0; // 2 horas da primeira atividade + 1 hora da segunda atividade
            double actualHorasTrabalhadas = medico.HorasTrabalhadas;
            double tolerance = 0.00000001;

            Assert.IsTrue(Math.Abs(expectedHorasTrabalhadas - actualHorasTrabalhadas) < tolerance);
        }

        [TestMethod]
        public void DeveAtualizarRankingCorretamente()
        {
            var medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            var medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            var atividade1 = new Atividade(1, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico1 }, TipoAtividade.Consulta);
            var atividade2 = new Atividade(2, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), new List<Medico> { medico2 }, TipoAtividade.Cirurgia);

            medico1.AdicionarAtividade(atividade1);
            medico2.AdicionarAtividade(atividade2);

            var medicos = new List<Medico> { medico1, medico2 };
            medico1.AtualizarRanking(medicos);

            Assert.AreEqual(1, medico1.Ranking);
            Assert.AreEqual(2, medico2.Ranking);
        }

        [TestMethod]
        public void MedicoNaoEstaDisponivelComAtividadesSobrepostas()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade1 = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(2), new List<Medico> { medico }, TipoAtividade.Consulta);
            var atividade2 = new Atividade(2, DateTime.Now.AddHours(1), DateTime.Now.AddHours(3), new List<Medico> { medico }, TipoAtividade.Cirurgia);

            medico.AdicionarAtividade(atividade1);

            Assert.ThrowsException<Exception>(() => medico.AdicionarAtividade(atividade2), "Médico não disponível");
        }

        [TestMethod]
        public void DeveAtualizarRankingComMedicosComHorasIguais()
        {
            var medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            var medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            var dataInicio = new DateTime(2023, 10, 1, 8, 0, 0);
            var dataFim = new DateTime(2023, 10, 1, 10, 0, 0);

            var atividade1 = new Atividade(1, dataInicio, dataFim, new List<Medico> { medico1 }, TipoAtividade.Consulta);
            var atividade2 = new Atividade(2, dataInicio, dataFim, new List<Medico> { medico2 }, TipoAtividade.Cirurgia);

            medico1.AdicionarAtividade(atividade1);
            medico2.AdicionarAtividade(atividade2);

            var medicos = new List<Medico> { medico1, medico2 };
            medico1.AtualizarRanking(medicos);

            Assert.AreEqual(1, medico1.Ranking);
            Assert.AreEqual(1, medico2.Ranking);
        }

        [TestMethod]
        public void DeveCalcularHorasTrabalhadasSemAtividades()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            medico.CalcularHorasTrabalhadas();

            Assert.AreEqual(0, medico.HorasTrabalhadas);
        }
    }
}


