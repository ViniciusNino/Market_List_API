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

        [NotMapped]
        public virtual ICollection<Lista> Listas { get; set; }
        [NotMapped]
        public virtual ICollection<UsuarioUnidade> UsuarioUnidades { get; set; }
        [NotMapped]
        public virtual ICollection<Item> Itens { get; set; }
        public override bool IsValid() => true;
    }
}
