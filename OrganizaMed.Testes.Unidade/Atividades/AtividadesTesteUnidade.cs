using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentResults;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Aplicacao.Servicos;

namespace OrganizaMed.Testes.Unidade.Atividades
{
    [TestClass]
    [TestCategory("Unidade")]
    public class AtividadesTesteUnidade
    {
        [TestMethod]
        public void DeveAdicionarAtividade()
        {
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Consulta);
            Assert.IsNotNull(atividade);
            Assert.AreEqual(TipoAtividade.Consulta, atividade.TipoAtividade);
        }

        [TestMethod]
        public void DeveAtualizarAtividade()
        {
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Consulta);
            var novaDataInicio = DateTime.Now.AddDays(1);
            var novaDataFim = DateTime.Now.AddDays(1).AddHours(1);

            atividade.DataInicio = novaDataInicio;
            atividade.DataFim = novaDataFim;

            Assert.AreEqual(novaDataInicio, atividade.DataInicio);
            Assert.AreEqual(novaDataFim, atividade.DataFim);
        }

        [TestMethod]
        public void DeveRemoverAtividade()
        {
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Consulta);
            atividade = null;

            Assert.IsNull(atividade);
        }

        [TestMethod]
        public void DeveObterAtividadePorId()
        {
            var atividade = new Atividade(1, DateTime.Now, DateTime.Now.AddHours(1), new List<Medico>(), TipoAtividade.Consulta);
            Assert.AreEqual(1, atividade.Id);
        }

        [TestMethod]
        public void DeveObterTodasAtividades()
        {
            var atividades = new List<Atividade>
            {
                new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Consulta),
                new Atividade(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), TipoAtividade.Cirurgia)
            };

            Assert.AreEqual(2, atividades.Count);
        }

        [TestMethod]
        public void DataInicioMaiorQueDataFim()
        {
            var atividade = new Atividade(DateTime.Now.AddHours(1), DateTime.Now, TipoAtividade.Consulta);
            var erros = atividade.Validar();

            Assert.IsTrue(erros.Contains("Data de início maior que data de fim"));
        }

        [TestMethod]
        public void DeveObterTempoRecuperacaoParaCirurgia()
        {
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Cirurgia);
            var tempoRecuperacao = atividade.ObterTempoRecuperacao();

            Assert.AreEqual(TimeSpan.FromHours(4), tempoRecuperacao);
        }

        [TestMethod]
        public void DeveObterTempoRecuperacaoParaConsulta()
        {
            var atividade = new Atividade(DateTime.Now, DateTime.Now.AddHours(1), TipoAtividade.Consulta);
            var tempoRecuperacao = atividade.ObterTempoRecuperacao();

            Assert.AreEqual(TimeSpan.FromMinutes(10), tempoRecuperacao);
        }
    }
}