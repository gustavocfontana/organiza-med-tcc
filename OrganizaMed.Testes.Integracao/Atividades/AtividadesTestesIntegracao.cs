using FizzWare.NBuilder;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Infra.Atividades;
using OrganizaMed.Infra.Compartilhado;

namespace OrganizaMed.Testes.Integracao.Atividades
{
    [TestClass]
    [TestCategory("Integracao")]
    public class AtividadesTestesIntegracao
    {
        private OrganizaMedDbContext dbContext;
        private RepositorioAtividades repositorio;

        [TestInitialize]
        public void Inicializar()
        {
            dbContext = new OrganizaMedDbContext();

            dbContext.Atividades.RemoveRange(dbContext.Atividades);

            repositorio = new RepositorioAtividades(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Atividade>(repositorio.Adicionar);
        }

        [TestMethod]
        public void DeveInserirAtividade()
        {
            var atividade = Builder<Atividade>
                .CreateNew()
                .Persist();

            var atividadeSelecionada = repositorio.ObterPorId(atividade.Id);

            Assert.IsNotNull(atividadeSelecionada);

            Assert.AreEqual(atividade, atividadeSelecionada);
        }
    }
}
