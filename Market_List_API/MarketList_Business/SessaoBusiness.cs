using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class SessaoBusiness : BaseBusiness<Sessao>, ISessaoBusiness
    {
        private readonly ISessaoRepository _sessaoRepository;

        public SessaoBusiness(ISessaoRepository sessaoRepository, IUnidadeDeTrabalho unidadeTrab) : base(sessaoRepository, unidadeTrab)
        {
            _sessaoRepository = sessaoRepository;
        }
    }
}
