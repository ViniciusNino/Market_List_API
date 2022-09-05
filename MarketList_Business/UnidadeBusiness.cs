using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class UnidadeBusiness : BaseBusiness<Unidade>, IUnidadeBusiness
    {
        private readonly IUnidadeRepository _unidadeRep;

        public UnidadeBusiness(IUnidadeRepository unidadeRepo) : base(unidadeRepo)
        {
            _unidadeRep = unidadeRepo;
        }

        public async Task<List<UnidadeVM>> GetUnidadesEListas(int usuarioId)
        {
            var unidadesDto = await _unidadeRep.GetUnidadesEListas(usuarioId);

            return MontarUnidadeVM(unidadesDto);
        }

        private List<UnidadeVM> MontarUnidadeVM(List<UnidadeDTO> unidadesDto)
        {
            var unidadesVM = unidadesDto
                    .GroupBy(x => new { x.Id, x.Nome })
                    .Select(y => new UnidadeVM
                    {
                        Id = y.Key.Id,
                        Nome = y.Key.Nome,
                        Lista = unidadesDto.Where(x => x.Id == y.Key.Id)
                                        .Select(x => new ListaVM
                                        {
                                            Id = x.ListaId,
                                            Cadastro = x.Cadastro,
                                            Nome = x.NomeLista
                                        }).ToList()
                    }).ToList();

            return unidadesVM;
        }
    }
}