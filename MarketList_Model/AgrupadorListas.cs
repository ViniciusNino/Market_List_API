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
        [NotMapped]
        public virtual Usuario Usuario { get; set; }
        [NotMapped]
        public virtual Status Status { get; set; }
    }

    public enum StatusAgrupadoEnum
    {
        Ativo = 8
    }
}
