using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class ItemLista : Entity<ItemLista>
    {
        public int NIdLista { get; set; }
        public int NIdItem { get; set; }
        public decimal NQuantidade { get; set; }
        public DateTime DCadastro { get; set; }
        public bool BAtivo { get; set; }
        public int NIdStatus { get; set; }
        public int NIdUsuarioSolicitante { get; set; }
        public int? NIdUsuarioComprador { get; set; }

        [NotMapped]
        public virtual Item Item { get; set; }

        [NotMapped]
        public virtual Lista Lista { get; set; }

        [NotMapped]
        public virtual Status Status { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioComprador { get; set; }

        [NotMapped]
        public virtual Usuario UsuarioSolicitante { get; set; }

        public override bool IsValid() => true;
    }

    public enum StatusItemListaEnum
    {
        Solicitado = 2,
        Atualizado = 3,
        Excluido = 4
    }
}
