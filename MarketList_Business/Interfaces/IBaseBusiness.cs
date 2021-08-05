using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketList_Business
{
    public interface IBaseBusiness<T> where T : class
    {
        Task<IEnumerable<T>> List();
        Task<T> GetId(int id);
        Task<T> Adicionar(T item);
        Task<int> AdicionarLista(List<T> listaItem);
        Task<int> Atualizar(T item);
        Task<int> AtualizarLista(List<T> listaItem);
        Task<int> Remover(T item);
        Task<int> RemoverLista(List<T> listaItem);
    }
}