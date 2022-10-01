using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MarketList_Model
{
    public partial class ListaAgrupadorListas : ModelBase
    {
        public int NIdAgrupadorListas { get; set; }
        public int NIdLista { get; set; }

        [NotMapped]
        public virtual AgrupadorListas AgrupadorListas { get; set; }
        [NotMapped]
        public virtual Lista Lista { get; set; }
    }
}
