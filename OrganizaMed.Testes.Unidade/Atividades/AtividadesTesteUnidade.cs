using OrganizaMed.Dominio.Atividades;

namespace OrganizaMed.Testes.Unidade.Atividades
{
    [TestClass]
    [TestCategory("Unidade")]
    public class AtividadesTesteUnidade
    {
        [TestMethod]
        public void DataInicioMaiorQueDataFim()
        {
            Atividade ? atividade = new Atividade(1, DateTime.Now.AddHours(1), DateTime.Now, new List<Medico>(), TipoAtividade.Consulta);
            List<string> ? erros = atividade.Validar();

            Assert.IsTrue(erros.Contains("Data de início maior que data de fim"));
        }

        [TestMethod]
        public void DeveObterTempoRecuperacaoParaCirurgia()
        {
            Atividade ? atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(1), new List<Medico>(), TipoAtividade.Cirurgia);
            TimeSpan tempoRecuperacao = atividade.ObterTempoRecuperacao();

            Assert.AreEqual(TimeSpan.FromHours(4), tempoRecuperacao);
        }

        [TestMethod]
        public void DeveObterTempoRecuperacaoParaConsulta()
        {
            Atividade ? atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(1), new List<Medico>(), TipoAtividade.Consulta);
            TimeSpan tempoRecuperacao = atividade.ObterTempoRecuperacao();

            Assert.AreEqual(TimeSpan.FromMinutes(10), tempoRecuperacao);
        }
    }
}
