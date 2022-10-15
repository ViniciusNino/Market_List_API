using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using FluentValidation;

namespace MarketList_Model
{
    [Serializable]
    public class Usuario : Entity<Usuario>
    {
        public Usuario()
        {
            PerfilUsuario = new PerfilUsuario();
            Status = new Status();
            ItensListaComprador = new HashSet<ItemLista>();
            ItensListaSolicitante = new HashSet<ItemLista>();
            Listas = new HashSet<Lista>();
            Agrupadores = new HashSet<Agrupador>();
        }
        public string SUsuario { get; set; }
        public string SNome { get; set; }
        public string SSenha { get; set; }
        public string SEmail { get; private set; }
        public int NIdPerfilUsuario { get; set; }
        public int NIdStatus { get; set; }
        public DateTime DCadastro { get; set; }

        public virtual PerfilUsuario PerfilUsuario { get; set; }

        public virtual Status Status { get; set; }

        public virtual ICollection<ItemLista> ItensListaComprador { get; set; }

        public virtual ICollection<ItemLista> ItensListaSolicitante { get; set; }

        public virtual ICollection<Lista> Listas { get; set; }

        public virtual ICollection<UsuarioUnidade> UsuarioUnidades { get; set; }

        public virtual ICollection<Agrupador> Agrupadores { get; set; }

        public override bool IsValid()
        {
            RuleFor(s => s.SEmail).NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email válido é obrigatório");

            RuleFor(c => c.SSenha)
                .NotEmpty().WithMessage("Senha é obrigatório")
                .MinimumLength(6).WithMessage("Senha precisa ter 6 characters");

            RuleFor(c => c.SNome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(3).WithMessage("Nome precisa ter 3 characters");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }

    public enum StatusUsuarioEnum
    {
        Ativo = 1,
        Aguardando_Ativacao_Email = 6
    }
}
