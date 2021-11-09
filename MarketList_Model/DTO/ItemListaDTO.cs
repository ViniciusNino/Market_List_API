namespace MarketList_Model
{
    public class ItemListaDTO
    {
        public int Id { get; set; }
     
        public int ListaId { get; set; }

        public int UsuarioLogadoId { get; set; }

        public int ItemId { get; set; }

        public string Nome { get; set; }

        public string UnidadeMedida { get; set; }
        
        public decimal Quantidade { get; set; }
    }
}