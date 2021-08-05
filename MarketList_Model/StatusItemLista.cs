using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class StatusItemLista : ModelBase
    {
        public StatusItemLista()
        {
            ItemLista = new HashSet<ItemLista>();
        }

        public string SDescricao { get; set; }

        public virtual ICollection<ItemLista> ItemLista { get; set; }
    }
}
