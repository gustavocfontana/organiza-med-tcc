using FluentResults;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Aplicacao.Servicos
{
    public class AtividadesServico
    {
        readonly private IRepositorioAtividades repositorioAtividade;
        readonly private IRepositorioMedicos repositorioMedico;

        public AtividadesServico(IRepositorioAtividades repositorioAtividade, IRepositorioMedicos repositorioMedico)
        {
            this.repositorioAtividade = repositorioAtividade;
            this.repositorioMedico = repositorioMedico;
        }

        public Result<Atividade> Adicionar(Atividade atividade)
        {
            Atividade ? atividadeExistente = repositorioAtividade.ObterTodos().FirstOrDefault(a =>
                a.DataInicio == atividade.DataInicio &&
                a.DataFim == atividade.DataFim &&
                a.TipoAtividade == atividade.TipoAtividade);

            if (atividadeExistente != null)
                return Result.Fail<Atividade>("Atividade já cadastrada.");

            // Registra os médicos primeiro se ainda não existirem
            foreach (Medico medico in atividade.MedicosEnvolvidos)
            {
                Medico medicoExistente = repositorioMedico.ObterPorId(medico.Id);
                if (medicoExistente == null)
                    repositorioMedico.Adicionar(medico);
            }

            // Adiciona a atividade
            repositorioAtividade.Adicionar(atividade);

            // Atualiza as horas trabalhadas para cada médico envolvido
            foreach (Medico medico in atividade.MedicosEnvolvidos)
            {
                Medico medicoAtual = repositorioMedico.ObterPorId(medico.Id);
                if (medicoAtual != null)
                {
                    medicoAtual.AdicionarAtividade(atividade);
                    medicoAtual.CalcularHorasTrabalhadas();
                    repositorioMedico.Atualizar(medicoAtual);
                }
            }

            return Result.Ok(atividade);
        }

        public Result<Atividade> Atualizar(Atividade atividadeAtualizada)
        {
            Atividade atividade = repositorioAtividade.ObterPorId(atividadeAtualizada.Id);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            atividade.DataInicio = atividadeAtualizada.DataInicio;
            atividade.DataFim = atividadeAtualizada.DataFim;
            atividade.MedicosEnvolvidos = atividadeAtualizada.MedicosEnvolvidos;

            repositorioAtividade.Atualizar(atividade);

            return Result.Ok(atividade);
        }

        public Result<Atividade> Remover(int atividadeId)
        {
            Atividade atividade = repositorioAtividade.ObterPorId(atividadeId);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            repositorioAtividade.Remover(atividade);

            return Result.Ok(atividade);
        }

        public Result<Atividade> ObterPorId(int atividadeId)
        {
            Atividade atividade = repositorioAtividade.ObterPorId(atividadeId);

            if (atividade == null)
                return Result.Fail<Atividade>("Atividade não encontrada");

            atividade.MedicosEnvolvidos = repositorioAtividade.ObterMedicosEnvolvidos(atividadeId);

            return Result.Ok(atividade);
        }

        public Result<List<Atividade>> ObterTodos()
        {
            List<Atividade> atividades = repositorioAtividade.ObterTodos();
            return Result.Ok(atividades);
        }
    }
}
