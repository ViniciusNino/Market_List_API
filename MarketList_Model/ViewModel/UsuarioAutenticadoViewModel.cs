namespace MarketList_Model
{
    public class UsuarioAutenticadoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int PerfilId { get; set; }

        public int? UnidadeId { get; set; }

        public string NomeUnidade { get; set; }
    }
}