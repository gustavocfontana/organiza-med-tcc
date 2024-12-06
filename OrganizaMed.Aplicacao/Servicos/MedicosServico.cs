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
    public class MedicosServico
    {
        private readonly IRepositorioMedicos repositorioMedico;
        private readonly IRepositorioAtividades repositorioAtividade;

        public MedicosServico(IRepositorioMedicos repositorioMedico)
        {
            this.repositorioMedico = repositorioMedico;
            this.repositorioAtividade = repositorioAtividade;
        }

        public Result<Medico> Adicionar(Medico medico)
        {
            var medicoExistente = repositorioMedico.ObterTodos().FirstOrDefault(m => m.Crm == medico.Crm);
            if (medicoExistente != null)
                return Result.Fail<Medico>("Médico já cadastrado.");

            repositorioMedico.Adicionar(medico);
            return Result.Ok(medico);
        }

        public Result<Medico> Atualizar(Medico medicoAtualizado)
        {
            var medico = repositorioMedico.ObterPorId
                (medicoAtualizado.Id);

            if (medico == null)
                return Result.Fail<Medico>("Médico não encontrado");

            medico.Nome = medicoAtualizado.Nome;
            medico.Crm = medicoAtualizado.Crm;

            repositorioMedico.Atualizar(medico);

            return Result.Ok(medico);
        }

        public Result<Medico> Remover(int medicoId)
        {
            var medico = repositorioMedico.ObterPorId(medicoId);

            if (medico == null)
                return Result.Fail<Medico>("Médico não encontrado");

            repositorioMedico.Remover(medico);

            return Result.Ok(medico);
        }

        public Result<Medico> ObterPorId(int medicoId)
        {
            var medico = repositorioMedico.ObterPorId(medicoId);

            if (medico == null)
                return Result.Fail<Medico>("Médico não encontrado");

            return Result.Ok(medico);
        }

        public Result<List<Medico>> ObterTodos()
        {
            var medicos = repositorioMedico.ObterTodos();

            return Result.Ok(medicos);
        }

        public IEnumerable<string> VerificarDisponibilidade(IEnumerable<int> medicoIds, DateTime dataInicio, DateTime dataFim)
        {
            var medicosIndisponiveis = new List<string>();

            foreach (var medicoId in medicoIds)
            {
                var medico = ObterPorId(medicoId).Value;
                if (medico != null && !medico.EstaDisponivel(dataInicio, dataFim))
                {
                    medicosIndisponiveis.Add(medico.Nome);
                }
            }

            return medicosIndisponiveis;
        }

        public void AtualizarRanking()
        {
            var medicos = repositorioMedico.ObterTodos();

            foreach (var medico in medicos)
            {
                medico.CalcularHorasTrabalhadas();
            }

            var medicosOrdenados = medicos.OrderByDescending(m => m.HorasTrabalhadas).ToList();

            for ( int i = 0 ; i < medicosOrdenados.Count ; i++ )
            {
                medicosOrdenados[i].Ranking = i + 1;
                repositorioMedico.Atualizar(medicosOrdenados[i]);
            }
        }
    }
}
