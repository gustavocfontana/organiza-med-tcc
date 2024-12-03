using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Aplicacao.Servicos
{
    public class AtividadesServico
    {
        private readonly IRepositorioAtividades repositorioAtividade;
        private readonly IRepositorioMedicos repositorioMedico;

        public AtividadesServico(IRepositorioAtividades repositorioAtividade, IRepositorioMedicos repositorioMedico)
        {
            this.repositorioAtividade = repositorioAtividade;
            this.repositorioMedico = repositorioMedico;
        }

        public Result<Atividade> Adicionar(Atividade atividade)
        {
            var atividadeExistente = repositorioAtividade.ObterTodos().FirstOrDefault(a =>
                a.DataInicio == atividade.DataInicio &&
                a.DataFim == atividade.DataFim &&
                a.TipoAtividade == atividade.TipoAtividade);

            if (atividadeExistente != null)
                return Result.Fail<Atividade>("Atividade já cadastrada.");

            // Adiciona a atividade
            repositorioAtividade.Adicionar(atividade);

            // Atualiza as horas trabalhadas para cada médico envolvido
            foreach (var medico in atividade.MedicosEnvolvidos)
            {
                medico.CalcularHorasTrabalhadas();
                // Atualiza o médico no repositório
                repositorioMedico.Atualizar(medico);
            }

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
