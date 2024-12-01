using FluentResults;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace OrganizaMed.Aplicacao.Servicos
{
    public class MedicosServico
    {
        readonly private IRepositorioAtividades repositorioAtividade;
        readonly private IRepositorioMedicos repositorioMedico;

        public MedicosServico(IRepositorioMedicos repositorioMedico)
        {
            this.repositorioMedico = repositorioMedico;
            repositorioAtividade = repositorioAtividade;
        }

        public Result<Medico> Adicionar(Medico medico)
        {
            Medico ? medicoExistente = repositorioMedico.ObterTodos().FirstOrDefault(m => m.Crm == medico.Crm);
            if (medicoExistente != null)
                return Result.Fail<Medico>("Médico já cadastrado.");

            repositorioMedico.Adicionar(medico);
            return Result.Ok(medico);
        }

        public Result<Medico> Atualizar(Medico medicoAtualizado)
        {
            Medico medico = repositorioMedico.ObterPorId
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
            Medico medico = repositorioMedico.ObterPorId(medicoId);

            if (medico == null)
                return Result.Fail<Medico>("Médico não encontrado");

            repositorioMedico.Remover(medico);

            return Result.Ok(medico);
        }

        public Result<Medico> ObterPorId(int medicoId)
        {
            Medico medico = repositorioMedico.ObterPorId(medicoId);

            if (medico == null)
                return Result.Fail<Medico>("Médico não encontrado");

            return Result.Ok(medico);
        }

        public Result<List<Medico>> ObterTodos()
        {
            List<Medico> medicos = repositorioMedico.ObterTodos();

            return Result.Ok(medicos);
        }

        public IEnumerable<string> VerificarDisponibilidade(IEnumerable<int> medicoIds, DateTime dataInicio, DateTime dataFim)
        {
            var medicosIndisponiveis = new List<string>();

            foreach (int medicoId in medicoIds)
            {
                Medico medico = ObterPorId(medicoId).Value;
                if (medico.Atividades.Any(a => a.DataInicio < dataFim && a.DataFim > dataInicio))
                    if (medico != null && !medico.EstaDisponivel(dataInicio, dataFim))
                        medicosIndisponiveis.Add(medico.Nome);
            }

            return medicosIndisponiveis;
        }

        public void AtualizarRanking()
        {
            List<Medico> medicos = repositorioMedico.ObterTodos();

            foreach (Medico medico in medicos)
                medico.CalcularHorasTrabalhadas();

            var medicosOrdenados = medicos.OrderByDescending(m => m.HorasTrabalhadas).ToList();

            for ( int i = 0 ; i < medicosOrdenados.Count ; i++ )
            {
                medicosOrdenados[i].Ranking = i + 1;
                repositorioMedico.Atualizar(medicosOrdenados[i]);
            }
        }
    }
}
