using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class Unidade : ModelBase
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
    }
}
