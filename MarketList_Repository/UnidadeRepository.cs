using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace MarketList_Repository
{
    public class UnidadeRepository : BaseRepository<Unidade>, IUnidadeRepository
    {
        private readonly MarketListContext _context;

        public UnidadeRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UnidadeDTO>> GetUnidadesEListas(int usuarioId)
        {
            using (_context)
            {
                return await (from uni in _context.Unidade
                              join unUni in _context.UsuarioUnidade on uni.Id equals unUni.NIdUnidade
                              join li in _context.Lista on uni.Id equals li.NIdUnidade
                              where unUni.NIdUsuario == usuarioId
                              select new UnidadeDTO
                              {
                                  Id = uni.Id,
                                  Nome = uni.SNome,
                                  Cadastro = li.DCadastro,
                                  ListaId = li.Id,
                                  NomeLista = li.SNome
                              }).ToListAsync();
            }

        }
    }
}