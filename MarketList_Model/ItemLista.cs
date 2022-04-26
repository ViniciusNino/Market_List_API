using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class ItemLista : ModelBase
    {
        public int NIdLista { get; set; }
        public int NIdItem { get; set; }
        public decimal NQuantidade { get; set; }
        public DateTime DCadastro { get; set; }
        public bool BAtivo { get; set; }
        public int NIdStatusItemLista { get; set; }
        public int NIdUsuarioSolicitante { get; set; }
        public int? NIdUsuarioComprador { get; set; }

        [NotMapped]
        public virtual Item Item { get; set; }

        [NotMapped]
        public virtual Lista Lista { get; set; }

        [NotMapped]
        public virtual StatusItemLista StatusItemLista { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioComprador { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioSolicitante { get; set; }
    }

    public enum EStatusItemLista
    {
        Solicitado = 1
    }
}
