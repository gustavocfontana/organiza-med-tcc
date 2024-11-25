using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var medico = new Medico(
                "Fulano",
                "12345-SP",
                "Cardio");

            var erros = medico.Validar();

            Assert.AreEqual(0, erros.Count);
        }

        [TestMethod]
        public void DeveCriarErro()
        {
            var medico = new Medico(
                "Fu",
                "12345-S",
                "Cardio");

            var erros = medico.Validar();

            List<string> errosEsperados = ["Nome inválido", "CRM inválido"];

            Assert.AreEqual(2, erros.Count);
        }

        [TestMethod]
        public void MedicoEstaDisponivel()
        {
            var medico = new Medico(
                "Fulano",
                "12345-SP",
                "Cardio");

            medico.AdicionarAtividade(new Consulta
            {
                DataInicio = DateTime.Now.AddHours(1),
                DataFim = DateTime.Now.AddHours(2)
            });

            var disponivel = medico.EstaDisponivel(
                DateTime.Now.AddHours(1.5),
                DateTime.Now.AddHours(3)
            );

            Assert.IsFalse(disponivel);
        }
    }
}


