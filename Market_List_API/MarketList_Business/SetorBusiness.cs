using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class SetorBusiness : BaseBusiness<Setor>, ISetorBusiness
    {
        private readonly ISetorRepository _setorRepository;

        public SetorBusiness(ISetorRepository setorRepository, IUnidadeDeTrabalho unidadeTrab) : base(setorRepository, unidadeTrab)
        {
            _setorRepository = setorRepository;
        }
    }
}
