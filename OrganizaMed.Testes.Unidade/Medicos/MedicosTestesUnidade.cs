using OrganizaMed.Dominio.Atividades;

namespace OrganizaMed.Testes.Unidade.Medicos
{
    [TestClass]
    [TestCategory("Unidade")]
    public class MedicosTestesUnidade
    {
        [TestMethod]
        public void DeveCriarValida()
        {
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            Assert.IsNotNull(medico);
            Assert.AreEqual("João Silva", medico.Nome);
            Assert.AreEqual("12345678", medico.Crm);
            Assert.AreEqual("Cardiologia", medico.Especialidade);
        }

        [TestMethod]
        public void DeveCriarErro()
        {
            Medico ? medico = new Medico("Jo", "1234", "");
            List<string> ? erros = medico.Validar();
            Assert.IsTrue(erros.Contains("Nome inválido"));
            Assert.IsTrue(erros.Contains("CRM inválido"));
            Assert.IsTrue(erros.Contains("Especialidade inválida"));
        }

        [TestMethod]
        public void MedicoEstaDisponivelConsultaRecuperacao()
        {
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            Atividade ? atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddMinutes(30), new List<Medico> { medico }, TipoAtividade.Consulta);
            medico.AdicionarAtividade(atividade);

            bool disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2));
            Assert.IsTrue(disponibilidade);
        }

        [TestMethod]
        public void MedicoEstaDisponivelCirurgiaRecuperacao()
        {
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            Atividade ? atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(2), new List<Medico> { medico }, TipoAtividade.Cirurgia);
            medico.AdicionarAtividade(atividade);

            bool disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(5), DateTime.Now.AddHours(6));
            Assert.IsFalse(disponibilidade);
        }

        [TestMethod]
        public void DeveCalcularHorasTrabalhadasCorretamente()
        {
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            Atividade ? atividade1 = new Atividade(1, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico }, TipoAtividade.Consulta);
            Atividade ? atividade2 = new Atividade(2, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), new List<Medico> { medico }, TipoAtividade.Cirurgia);
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
            Medico ? medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            Medico ? medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            Atividade ? atividade1 = new Atividade(1, DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-3), new List<Medico> { medico1 }, TipoAtividade.Consulta);
            Atividade ? atividade2 = new Atividade(2, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), new List<Medico> { medico2 }, TipoAtividade.Cirurgia);

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
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            Atividade ? atividade1 = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(2), new List<Medico> { medico }, TipoAtividade.Consulta);
            Atividade ? atividade2 = new Atividade(2, DateTime.Now.AddHours(1), DateTime.Now.AddHours(3), new List<Medico> { medico }, TipoAtividade.Cirurgia);

            medico.AdicionarAtividade(atividade1);

            Assert.ThrowsException<Exception>(() => medico.AdicionarAtividade(atividade2), "Médico não disponível");
        }

        [TestMethod]
        public void DeveAtualizarRankingComMedicosComHorasIguais()
        {
            Medico ? medico1 = new Medico("João Silva", "12345678", "Cardiologia");
            Medico ? medico2 = new Medico("Maria Souza", "87654321", "Pediatria");

            DateTime dataInicio = new DateTime(2023, 10, 1, 8, 0, 0);
            DateTime dataFim = new DateTime(2023, 10, 1, 10, 0, 0);

            Atividade ? atividade1 = new Atividade(1, dataInicio, dataFim, new List<Medico> { medico1 }, TipoAtividade.Consulta);
            Atividade ? atividade2 = new Atividade(2, dataInicio, dataFim, new List<Medico> { medico2 }, TipoAtividade.Cirurgia);

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
            Medico ? medico = new Medico("João Silva", "12345678", "Cardiologia");
            medico.CalcularHorasTrabalhadas();

            Assert.AreEqual(0, medico.HorasTrabalhadas);
        }
    }
}
