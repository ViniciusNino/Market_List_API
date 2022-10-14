using System;
using System.Threading.Tasks;

namespace MarketList_Repository
{
    public interface IUnidadeDeTrabalho : IDisposable
    {
        Task<bool> CommitAsync();
    }
}