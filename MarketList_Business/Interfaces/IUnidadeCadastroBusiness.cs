using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IUnidadeCadastroBusiness : IBaseBusiness<Unidade>
    {
        Task<bool> SetUnidade(UnidadeCadastroDTO unidadeCadastro);
    }
}