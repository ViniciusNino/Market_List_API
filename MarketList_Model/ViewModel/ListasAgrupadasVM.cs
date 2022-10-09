using System.Collections.Generic;

namespace MarketList_Model
{
    public class ListasAgrupadasVM
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<ItensListasAgrupadasVM> Itens { get; set; }
    }
}