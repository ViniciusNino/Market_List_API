using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Tipo : Entity<Tipo>
    {
        public Tipo()
        {
            Usuarios = new HashSet<Usuario>();
            Tokens = new HashSet<VerificacaoToken>();
        }

        public int NIdArea { get; set; }
        public string SDescricao { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<VerificacaoToken> Tokens { get; set; }
        public override bool IsValid() => true;
    }
}
