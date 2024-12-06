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
    try
    {
        // 1. Verificar se já existe uma atividade igual
        var atividadeExistente = repositorioAtividade.ObterTodos()
            .FirstOrDefault(a =>
                a.DataInicio == atividade.DataInicio &&
                a.DataFim == atividade.DataFim &&
                a.TipoAtividade == atividade.TipoAtividade);

        if (atividadeExistente != null)
            return Result.Fail<Atividade>("Atividade já cadastrada.");

        // 2. Para cada médico, verificar disponibilidade
        foreach (var medico in atividade.MedicosEnvolvidos)
        {
            var medicoCompleto = repositorioMedico.ObterPorId(medico.Id);
            if (medicoCompleto == null)
                return Result.Fail<Atividade>($"Médico ID {medico.Id} não encontrado.");

            // Verificar conflito com atividades existentes
            foreach (var atividadeExistenteMedico in medicoCompleto.Atividades)
            {
                // Considerar o tempo de recuperação
                var fimComRecuperacao = atividadeExistenteMedico.DataFim + atividadeExistenteMedico.ObterTempoRecuperacao();

                // Verificar sobreposição considerando a data completa
                if (atividade.DataInicio < fimComRecuperacao && 
                    atividadeExistenteMedico.DataInicio < atividade.DataFim &&
                    atividade.DataInicio.Date == atividadeExistenteMedico.DataInicio.Date) // Comparar apenas se for no mesmo dia
                {
                    return Result.Fail<Atividade>(
                        $"Médico {medicoCompleto.Nome} não está disponível no horário selecionado. " +
                        $"Ele tem outra atividade no dia {atividadeExistenteMedico.DataInicio:dd/MM/yyyy} " +
                        $"das {atividadeExistenteMedico.DataInicio:HH:mm} até {atividadeExistenteMedico.DataFim:HH:mm}");
                }
            }
        }

        // 3. Se todos os médicos estiverem disponíveis, adicionar a atividade
        repositorioAtividade.Adicionar(atividade);

        // 4. Atualizar as horas trabalhadas dos médicos
        foreach (var medico in atividade.MedicosEnvolvidos)
        {
            var medicoCompleto = repositorioMedico.ObterPorId(medico.Id);
            medicoCompleto.CalcularHorasTrabalhadas();
            repositorioMedico.Atualizar(medicoCompleto);
        }

        return Result.Ok(atividade);
    }
    catch (Exception ex)
    {
        return Result.Fail<Atividade>($"Erro ao adicionar atividade: {ex.Message}");
    }
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
