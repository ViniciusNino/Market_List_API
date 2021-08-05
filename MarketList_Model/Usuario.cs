using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Usuario : ModelBase
    {
        public Usuario()
        {
            PerfilUsuario = new PerfilUsuario();
            StatusUsuario = new StatusUsuario();
            ItemListaNIdUsuarioCompradorNavigation = new HashSet<ItemLista>();
            ItemListaNIdUsuarioSolicitanteNavigation = new HashSet<ItemLista>();
            Lista = new HashSet<Lista>();
        }
        public string SUsuario { get; set; }
        public string SSenha { get; set; }
        public int NIdPerfilUsuario { get; set; }
        public int NIdStatusUsuario { get; set; }
        public int NIdUnidade { get; set; }

        [NotMapped]
        public virtual PerfilUsuario PerfilUsuario { get; set; }

        [NotMapped]
        public virtual StatusUsuario StatusUsuario { get; set; }

        [NotMapped]
        public virtual Unidade Unidade { get; set; }

        [NotMapped]
        public virtual ICollection<ItemLista> ItemListaNIdUsuarioCompradorNavigation { get; set; }

        [NotMapped]
        public virtual ICollection<ItemLista> ItemListaNIdUsuarioSolicitanteNavigation { get; set; }

        [NotMapped]
        public virtual ICollection<Lista> Lista { get; set; }
    }
}
