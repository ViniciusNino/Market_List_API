using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MarketList_Model;

#nullable disable

namespace MarketList_Model
{
    public partial class Status : ModelBase
    {
        public Status()
        {
            AgrupadorListas = new HashSet<AgrupadorListas>();
        }

        public int NIdArea { get; set; }
        public string SDescricao { get; set; }

        [NotMapped]
        public virtual Area NIdAreaNavigation { get; set; }
        [NotMapped]
        public virtual ICollection<AgrupadorListas> AgrupadorListas { get; set; }
    }
}
