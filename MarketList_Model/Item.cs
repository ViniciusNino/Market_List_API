using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model
{
    public partial class Item : ModelBase
    {
        public Item()
        {
            ItemLista = new HashSet<ItemLista>();
        }
        [Required(ErrorMessage = "Informe o nome do Item!")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "O nome deve ter no mínimo 3 e no máximo 255 caracter.")]
        public string SNome { get; set; }
        public int NIdSessao { get; set; }
        public int NIdUnidade { get; set; }

        [Required(ErrorMessage = "Informe a unidade de medida do Item!")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "O nome deve ter no mínimo 1 e no máximo 2 caracter.")]
        public string SUnidadeMedida { get; set; }

        [NotMapped]
        public virtual Unidade Unidade { get; set; }
        [NotMapped]
        public virtual Sessao Sessao { get; set; }
        [NotMapped]
        public virtual ICollection<ItemLista> ItemLista { get; set; }
    }
}
