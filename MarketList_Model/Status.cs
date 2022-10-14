using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MarketList_Model;

#nullable disable

namespace MarketList_Model
{
    public partial class Status : Entity<Status>
    {
        public int NIdArea { get; set; }
        public string SDescricao { get; set; }

        [NotMapped]
        public virtual Area Area { get; set; }
        public override bool IsValid() => true;
    }
}
