using OrganizaMed.Dominio.Atividades;

namespace OrganizaMed.Dominio.Compartilhado
{
    public interface IRepositorio<T> where T : EntidadeBase
    {
        T ObterPorId(int id);
        void Adicionar(T entidade);
        void Atualizar(T entidade);
        void Remover(T entidade);
        List<T> ObterTodos();
        List<Medico> ObterMedicosEnvolvidos(int atividadeId);
        List<Atividade> ObterAtividadesPorMedico(int medicoId);
    }
}