using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class PerfilUsuario : Entity<PerfilUsuario>
    {
        public PerfilUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public string SDescricao { get; set; }

        [NotMapped]
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public override bool IsValid() => true;
    }
}
