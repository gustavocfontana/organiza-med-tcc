using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using OrganizaMed.Dominio.Atividades;

namespace OrganizaMed.Aplicacao.Servicos
{
    public class AtividadesServico
    {
        private readonly IRepositorioAtividades repositorioAtividade;

        public AtividadesServico(IRepositorioAtividades repositorioAtividade)
        {
            this.repositorioAtividade = repositorioAtividade;
        }

        public Result<Atividade> Adicionar(Atividade atividade)
        {
            var atividadeExistente = repositorioAtividade.ObterTodos().FirstOrDefault(a =>
                a.DataInicio == atividade.DataInicio &&
                a.DataFim == atividade.DataFim &&
                a.TipoAtividade == atividade.TipoAtividade);

            if (atividadeExistente != null)
                return Result.Fail<Atividade>("Atividade já cadastrada.");

            repositorioAtividade.Adicionar(atividade);
            return Result.Ok(atividade);
        }

        public Result<Atividade> Atualizar(Atividade atividadeAtualizada)
        {
            var atividade = repositorioAtividade.ObterPorId
                (atividadeAtualizada.Id);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            atividade.DataInicio = atividadeAtualizada.DataInicio;
            atividade.DataFim = atividadeAtualizada.DataFim;

            repositorioAtividade.Atualizar(atividade);

            return Result.Ok(atividade);
        }

        public Result<Atividade> Remover(int atividadeId)
        {
            var atividade = repositorioAtividade.ObterPorId(atividadeId);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            repositorioAtividade.Remover(atividade);

            return Result.Ok(atividade);
        }

        public Result<Atividade> ObterPorId(int atividadeId)
        {
            var atividade = repositorioAtividade.ObterPorId(atividadeId);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            atividade.MedicosEnvolvidos = repositorioAtividade.ObterMedicosEnvolvidos(atividadeId);

            return Result.Ok(atividade);
        }

        public Result<List<Atividade>> ObterTodos()
        {
            var atividades = repositorioAtividade.ObterTodos();

            return Result.Ok(atividades);
        }
    }
}
