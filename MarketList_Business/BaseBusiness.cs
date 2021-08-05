using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Repository;

namespace MarketList_Business
{
    public class BaseBusiness<T> : IBaseBusiness<T> where T : class
    {
        private IBaseRepository<T> _repository;

        public BaseBusiness()
        { }

        public BaseBusiness(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<T> GetId(int id)
        {
            try
            {
                return _repository.GetId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("[GetId]", ex);
            }
        }

        public Task<IEnumerable<T>> List()
        {
            try
            {
                return _repository.List();
            }
            catch (Exception ex)
            {
                throw new Exception("[List]", ex);
            }
        }

        public Task<T> Adicionar(T item)
        {
            try
            {
                return _repository.Adicionar(item);
            }
            catch (Exception ex)
            {
                throw new Exception("[Adicionar]", ex);
            }
        }

        public Task<int> AdicionarLista(List<T> listaItem)
        {
            try
            {
                return _repository.AdicionarLista(listaItem);
            }
            catch (Exception ex)
            {
                throw new Exception("[AdicionarLista]", ex);
            }
        }

        public Task<int> Atualizar(T item)
        {
            try
            {
                return _repository.Atualizar(item);
            }
            catch (Exception ex)
            {
                throw new Exception("[Atualizar]", ex);
            }
        }

        public Task<int> AtualizarLista(List<T> listaItem)
        {
            try
            {
                return _repository.AtualizarLista(listaItem);
            }
            catch (Exception ex)
            {
                throw new Exception("[AtualizarLista]", ex);
            }
        }

        public Task<int> Remover(T item)
        {
            try
            {
                return _repository.Remover(item);
            }
            catch (Exception ex)
            {
                throw new Exception("[Remover]", ex);
            }
        }

        public Task<int> RemoverLista(List<T> listaItem)
        {
            try
            {
                return _repository.RemoverLista(listaItem);
            }
            catch (Exception ex)
            {
                throw new Exception("[RemoverLista]", ex);
            }
        }
    }
}