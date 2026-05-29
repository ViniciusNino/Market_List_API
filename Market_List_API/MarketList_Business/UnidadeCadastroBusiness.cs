using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class UnidadeCadastroBusiness : BaseBusiness<Unidade>, IUnidadeCadastroBusiness
    {
         private readonly IUsuarioUnidadeRepository _usuarioUnidadeRep;

         public UnidadeCadastroBusiness(IUsuarioUnidadeRepository usuarioUnidadeRep, IUnidadeRepository unidadeRepo, IUnidadeDeTrabalho unidadeTrab) : base(unidadeRepo, unidadeTrab)
         {
            _usuarioUnidadeRep = usuarioUnidadeRep;
         }

        public async Task<int> SetUnidade(UnidadeCadastroDTO unidadeCadastro)
        {
            var unidade = unidadeCadastro.GetUnidade();
            await AddAsync(unidade);
            await _unidadeTrab.CommitAsync();
            
            var usuarioUnidade = unidadeCadastro.GetUsuarioUnidade(unidade.Id);
            await _usuarioUnidadeRep.AddAsync(usuarioUnidade);

            await _unidadeTrab.CommitAsync();
            
            return unidade.Id;
        }
    }
}