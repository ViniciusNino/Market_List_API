using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Usuario : ModelBase
    {
        public Usuario()
        {
            PerfilUsuario = new PerfilUsuario();
            Status = new Status();
            ItemListaComprador = new HashSet<ItemLista>();
            ItemListaSolicitante = new HashSet<ItemLista>();
            Lista = new HashSet<Lista>();
        }
        public string SUsuario { get; set; }
        public string SSenha { get; set; }
        public int NIdPerfilUsuario { get; set; }
        public int NIdStatus { get; set; }
        public DateTime DCadastro { get; set; }

        [NotMapped]
        public virtual PerfilUsuario PerfilUsuario { get; set; }

        [NotMapped]
        public virtual Status Status { get; set; }

        [NotMapped]
        public virtual ICollection<ItemLista> ItemListaComprador { get; set; }

        [NotMapped]
        public virtual ICollection<ItemLista> ItemListaSolicitante { get; set; }

        [NotMapped]
        public virtual ICollection<Lista> Lista { get; set; }
    }
}
