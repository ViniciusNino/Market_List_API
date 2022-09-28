using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MarketList_Model;

#nullable disable

namespace MarketList_Model
{
    public partial class Area : ModelBase
    {
        public Area()
        {
            Statuses = new HashSet<Status>();
        }

        public string SDescricao { get; set; }

        [NotMapped]
        public virtual ICollection<Status> Statuses { get; set; }
    }
}
