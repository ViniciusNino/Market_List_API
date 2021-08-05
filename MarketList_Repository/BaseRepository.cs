using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private MarketListContext _context;

        public BaseRepository()
        { }

        public BaseRepository(MarketListContext context)
        {
            _context = context;
        }

        public async Task<T> GetId(int id)
        {
            try
            {
                using (_context)
                {
                    return await _context.Set<T>().FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[GetId]", ex);
            }
        }

        public async Task<IEnumerable<T>> List()
        {
            try
            {
                using (_context)
                {
                    return await _context.Set<T>().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[List]", ex);
            }
        }

        public async Task<T> Adicionar(T item)
        {
            try
            {
                  using (_context)
                {
                    await _context.Set<T>().AddAsync(item);
                    await _context.SaveChangesAsync();

                    return item;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Adicionar]", ex);
            }
        }

        public async Task<int> AdicionarLista(List<T> listaItem)
        {
            try
            {
                 using (_context)
                {
                    await _context.Set<T>().AddRangeAsync(listaItem);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[AdicionarLista]", ex);
            }
        }

        public async Task<int> Atualizar(T item)
        {
            try
            {
                using (_context)
                {
                    _context.Set<T>().Update(item);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Atualizar]", ex);
            }
        }

        public async Task<int> AtualizarLista(List<T> listaItem)
        {
            try
            {
               using (_context)
                {
                    _context.Set<T>().UpdateRange(listaItem);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[AtualizarLista]", ex);
            }
        }

        public async Task<int> Remover(T item)
        {
            try
            {
                using (_context)
                {
                    _context.Set<T>().Remove(item);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[Remover]", ex);
            }
           
        }

        public async Task<int> RemoverLista(List<T> listaItem)
        {
            try
            {
                using (_context)
                {
                    _context.Set<T>().RemoveRange(listaItem);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[RemoverLista]", ex);
            }
        }
    }
}