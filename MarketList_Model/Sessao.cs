using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Sessao : ModelBase
    {
        public Sessao()
        {
            Item = new HashSet<Item>();
        }

        public string SNome { get; set; }

        [NotMapped]
        public virtual ICollection<Item> Item { get; set; }
    }
}
