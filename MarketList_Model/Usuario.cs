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
            ItemListaComprador = new HashSet<ItemLista>();
            ItemListaSolicitante = new HashSet<ItemLista>();
            Lista = new HashSet<Lista>();
        }
        public string SUsuario { get; set; }
        public string SNome { get; set; }
        public string SSenha { get; set; }
        public string SEmail { get; set; }
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
}
