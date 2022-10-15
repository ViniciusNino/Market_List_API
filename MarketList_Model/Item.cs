using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace MarketList_Model
{
    public partial class Item : Entity<Item>
    {
        public Item()
        {
            ItensLista = new HashSet<ItemLista>();
        }
        public string SNome { get; set; }
        public string SUnidadeMedida { get; set; }
        public int NIdSessao { get; set; }
        public int NIdUnidade { get; set; }

        [NotMapped]
        public virtual Unidade Unidade { get; set; }
        [NotMapped]
        public virtual Sessao Sessao { get; set; }
        [NotMapped]
        public virtual ICollection<ItemLista> ItensLista { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.SUnidadeMedida)
                .NotEmpty().WithMessage("Unidade de Medida é obrigatório")
                .MinimumLength(2).MaximumLength(3).WithMessage("Mínimo 1 e máximo 3 characters");

            RuleFor(c => c.SNome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).MaximumLength(50).WithMessage("Mínimo 2 e máximo 50 characters");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
