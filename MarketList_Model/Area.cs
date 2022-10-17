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
            Tipos = new HashSet<Tipo>();
        }

        public string SDescricao { get; set; }

        public virtual ICollection<Status> ListaStatus { get; set; }
        public virtual ICollection<Tipo> Tipos { get; set; }

        public override bool IsValid() => true;
    }
}
