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
        public int NIdUsuario { get; }
        public int NIdTipo { get; }
        public string Token { get; }
        public DateTime DCadastro { get; private set; } = DateTime.Now;
        public bool BAtivo { get; private set; } = true;

        public virtual Usuario Usuario { get; set; }
        public virtual Tipo Tipo { get; set; }

        public bool NoPrazo()
        {
            BAtivo = DCadastro > DateTime.Now.AddHours(-1);

            return BAtivo;
        }
        public override bool IsValid()
        {
            RuleFor(s => s.Token).NotNull().NotEmpty()
                .MinimumLength(10).WithMessage("Token obrigatório!");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }

    public enum TipoVerificacaoTokenEnum
    {
        Ativacao_Email = 5,
        Alterar_Senha = 6
    }
}