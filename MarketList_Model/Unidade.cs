using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class Unidade : Entity<Unidade>
    {
        public Unidade()
        {
            Listas = new HashSet<Lista>();
            UsuarioUnidades = new HashSet<UsuarioUnidade>();
        }
        public string SNome { get; set; }

        public virtual ICollection<Lista> Listas { get; set; }
        public virtual ICollection<UsuarioUnidade> UsuarioUnidades { get; set; }
        public virtual ICollection<Item> Itens { get; set; }
        public override bool IsValid() => true;
    }
}
