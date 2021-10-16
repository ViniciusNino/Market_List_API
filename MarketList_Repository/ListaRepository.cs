using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class ListaRepository : BaseRepository<Lista>, IListaRepository
    {
        private readonly MarketListContext _contexto;

        public ListaRepository(MarketListContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            try
            {
                using (_contexto)
                {
                    return await _contexto.Lista.Where(x => x.NIdUnidade == unidadeId && x.BAtivo == true)
                                .Join(_contexto.Usuario, l => l.NIdUsuario, us => us.Id, (l, us) =>
                                new ListaDTO{
                                    dCadastro = l.DCadastro,
                                    nIdLista = l.Id,
                                    sNome = l.SNome,
                                    sNomeUsuario = us.SUsuario
                                }).ToListAsync();
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaRepository - GetListaPorUnidadeId] - " + ex.Message, ex);
            }
        }
    }
}