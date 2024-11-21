namespace OrganizaMed.Dominio.Compartilhado
{
    public interface IRepositorio<T> where T : EntidadeBase
    {
        T ObterPorId(int id);
        List<T> SelecionarTodos();
        void Adicionar(T entidade);
        void Atualizar(T entidade);
        void Remover(T entidade);
    }
}