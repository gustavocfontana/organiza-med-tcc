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
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddMinutes(30), TipoAtividade.Consulta);
            medico.AdicionarAtividade(atividade);

            var disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(1), DateTime.Now.AddHours(2));
            Assert.IsTrue(disponibilidade);
        }

        [TestMethod]
        public void MedicoEstaDisponivelCirurgiaRecuperacao()
        {
            var medico = new Medico("João Silva", "12345678", "Cardiologia");
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(2), TipoAtividade.Cirurgia);
            medico.AdicionarAtividade(atividade);

            var disponibilidade = medico.EstaDisponivel(DateTime.Now.AddHours(5), DateTime.Now.AddHours(6));
            Assert.IsFalse(disponibilidade);
        }
    }
}


