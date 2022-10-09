using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace MarketList_Model
{
    public partial class AgrupadorListas : ModelBase
    {
        public int NIdUsuario { get; set; }
        public int NIdStatus { get; set; }
        public DateTime DCadastro { get; set; }
        public string SDescricao { get; set; }

        [NotMapped]
        public virtual Status Status { get; set; }
        [NotMapped]
        public virtual Usuario Usuario { get; set; }
    }

    public enum StatusAgrupadoEnum
    {
        Ativo = 5
    }
}
