using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MarketList_Model
{
    public partial class ListaAgrupadorListas : ModelBase
    {
        public ListaAgrupadorListas()
        {
            InverseNIdAgrupadorListasNavigation = new HashSet<ListaAgrupadorListas>();
        }

        public int NIdAgrupadorListas { get; set; }
        public int NIdLista { get; set; }

        [NotMapped]
        public virtual ListaAgrupadorListas NIdAgrupadorListasNavigation { get; set; }
        [NotMapped]
        public virtual ICollection<ListaAgrupadorListas> InverseNIdAgrupadorListasNavigation { get; set; }
    }
}
