using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MarketList_Model;

#nullable disable

namespace MarketList_Model
{
    public partial class Status : Entity<Status>
    {
        public Status()
        {
            Usuarios = new HashSet<Usuario>();
            Agrupadores = new HashSet<Agrupador>();
            ItensLista = new HashSet<ItemLista>();
        }
        public int NIdArea { get; set; }
        public string SDescricao { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Agrupador> Agrupadores { get; set; }
        public virtual ICollection<ItemLista> ItensLista { get; set; }
        public override bool IsValid() => true;
    }
}
