using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public partial class Unidade : Entity<Unidade>
    {
        public Unidade()
        {
            Itens = new HashSet<Item>();
            Listas = new HashSet<Lista>();
            Usuarios = new HashSet<Usuario>();
            UsuarioUnidades = new HashSet<UsuarioUnidade>();
        }

        public Unidade(string nome)
        {
            SNome = nome;
        }
        public string SNome { get; set; }
        public DateTime DCadastro { get; set; }

        public virtual ICollection<Lista> Listas { get; set; }
        public virtual ICollection<UsuarioUnidade> UsuarioUnidades { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Item> Itens { get; set; }
        public override bool IsValid() => true;
    }
}
