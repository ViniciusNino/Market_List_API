using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MarketList_Model
{
    public partial class ListaAgrupador : Entity<ListaAgrupador>
    {
        public int NIdAgrupadorListas { get; set; }
        public int NIdLista { get; set; }

        [NotMapped]
        public virtual Agrupador Agrupador { get; set; }
        [NotMapped]
        public virtual Lista Lista { get; set; }
        public override bool IsValid() => true;
    }
}
