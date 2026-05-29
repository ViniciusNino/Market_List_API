using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Sessao : Entity<Sessao>
    {
        public Sessao()
        {
            Itens = new HashSet<Item>();
            Setores = new HashSet<Setor>();
        }

        public string SNome { get; set; }

        [NotMapped]
        public virtual ICollection<Item> Itens { get; set; }
        public virtual ICollection<Setor> Setores { get; set; }
        public override bool IsValid() => true;
    }
}
