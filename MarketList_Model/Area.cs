using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MarketList_Model;

#nullable disable

namespace MarketList_Model
{
    public partial class Area : Entity<Area>
    {
        public Area()
        {
            ListaStatus = new HashSet<Status>();
        }

        public string SDescricao { get; set; }

        [NotMapped]
        public virtual ICollection<Status> ListaStatus { get; set; }

        public override bool IsValid() => true;
    }
}
