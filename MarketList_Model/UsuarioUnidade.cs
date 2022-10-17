
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class UsuarioUnidade : Entity<UsuarioUnidade>
    {
        public int NIdUsuario { get; set; }
        public int NIdUnidade { get; set; }

        [NotMapped]
        public virtual Usuario Usuario { get; set; }
        [NotMapped]
        public virtual Unidade Unidade { get; set; }
        public override bool IsValid() => true;
    }

}
