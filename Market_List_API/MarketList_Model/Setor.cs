using System;
using System.Collections.Generic;
using FluentValidation;

namespace MarketList_Model
{
    [Serializable]
    public partial class Setor : Entity<Setor>
    {
        public Setor()
        {
            Itens = new HashSet<Item>();
        }

        public Setor(string nome, int idSessao) : this()
        {
            SNome = nome;
            NIdSessao = idSessao;
            DCadastro = DateTime.Now;
        }

        public string SNome { get; set; }
        public int NIdSessao { get; set; }
        public DateTime DCadastro { get; set; }

        public virtual Sessao Sessao { get; set; }
        public virtual ICollection<Item> Itens { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.SNome)
                .NotEmpty().WithMessage("Nome do setor é obrigatório")
                .MinimumLength(2).MaximumLength(50).WithMessage("Nome do setor deve ter entre 2 e 50 caracteres");

            RuleFor(c => c.NIdSessao)
                .GreaterThan(0).WithMessage("Estabelecimento (Sessão) inválido");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
