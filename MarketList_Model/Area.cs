using System.Collections.Generic;

#nullable disable

namespace MarketList_Model
{
    public partial class Area : Entity<Area>
    {
        public Area()
        {
            ListaStatus = new HashSet<Status>();
            Tipos = new HashSet<Tipo>();
        }

        public string SDescricao { get; set; }

        public virtual ICollection<Status> ListaStatus { get; set; }
        public virtual ICollection<Tipo> Tipos { get; set; }

        public enum AreaEnum
        {
            Usuario = 1,
            Agrupador = 2,
            Lista = 3
        }

        public override bool IsValid() => true;
    }
}
