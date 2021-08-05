using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketList_Repository
{
    public interface IBaseRepository<T> where T : class
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