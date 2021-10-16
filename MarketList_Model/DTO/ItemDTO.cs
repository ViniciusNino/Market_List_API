namespace MarketList_Model
{
    public class ItemDTO
    {
        public int Id { get; set; }
        
        public string Nome { get; set; }
        
        public string UnidadeMedida { get; set; }     

        public decimal Quantidade { get; set; }

        public int UsuarioLogadoId { get; set; }

        public int? ItemListaId { get; set; }
    }
}