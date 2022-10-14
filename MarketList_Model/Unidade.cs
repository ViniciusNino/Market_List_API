using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class Unidade : Entity<Unidade>
    {
        public Unidade()
        {
            Lista = new HashSet<Lista>();
            Usuario = new HashSet<Usuario>();
        }
        public string SNome { get; set; }

        [NotMapped]
        public virtual ICollection<Lista> Lista { get; set; }
        [NotMapped]
        public virtual ICollection<Usuario> Usuario { get; set; }
        public override bool IsValid() => true;
    }
}
