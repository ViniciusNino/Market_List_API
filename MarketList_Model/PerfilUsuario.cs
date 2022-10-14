using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class PerfilUsuario : Entity<PerfilUsuario>
    {
        public PerfilUsuario()
        {
            Usuario = new HashSet<Usuario>();
        }

        public string SDescricao { get; set; }

        [NotMapped]
        public virtual ICollection<Usuario> Usuario { get; set; }
        public override bool IsValid() => true;
    }
}
