using System;
using FluentValidation;

namespace MarketList_Model
{
    public class VerificacaoToken : Entity<VerificacaoToken>
    {
        public VerificacaoToken() { }

        public VerificacaoToken(int usuarioId, int tipoId, string token)
        {
            NIdUsuario = usuarioId;
            NIdTipo = tipoId;
            Token = token;
        }
        public int NIdUsuario { get; set; }
        public int NIdTipo { get; }
        public string Token { get; }
        public DateTime DCadastro { get; } = DateTime.Now;

        public virtual Usuario Usuario { get; set; }
        public virtual Tipo Tipo { get; set; }
        public override bool IsValid()
        {
            RuleFor(s => s.Token).NotNull().NotEmpty()
                .MinimumLength(10).WithMessage("Token obrigatório!");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}