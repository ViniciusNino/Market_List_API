using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class Lista : Entity<Lista>
    {
        public Lista()
        {
            ItensLista = new HashSet<ItemLista>();
            ListaAgrupador = new HashSet<ListaAgrupador>();
        }

        public int NIdUnidade { get; set; }
        public int NIdUsuario { get; set; }
        public bool BAtivo { get; set; }
        public DateTime DCadastro { get; set; }
        public string SNome { get; set; }
        [NotMapped]
        public virtual Unidade Unidade { get; set; }
        [NotMapped]
        public virtual Usuario Usuario { get; set; }
        [NotMapped]
        public virtual ICollection<ItemLista> ItensLista { get; set; }
        [NotMapped]
        public virtual ICollection<ListaAgrupador> ListaAgrupador { get; set; }
        public override bool IsValid() => true;
    }
}
