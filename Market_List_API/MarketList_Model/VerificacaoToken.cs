using System;
using System.Text;
using FluentValidation;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_Model;

public class VerificacaoToken : Entity<VerificacaoToken>
{
    private const int MinutosConfirmarEmail = 10;
    private const int QuantidadeCaracter = 10;
    public VerificacaoToken() { }

    public VerificacaoToken(int usuarioId, int tipoId)
    {
        NIdUsuario = usuarioId;
        NIdTipo = tipoId;
        Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
    }
    public int NIdUsuario { get; }
    public int NIdTipo { get; }
    public string Token { get;  private set; }
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

    public bool TokenValido()
    {
        var tempoEnvioEmail = (DateTime.Now - DCadastro).TotalMinutes;

        return tempoEnvioEmail <= MinutosConfirmarEmail;
    }

    public string GetTokenEmal()
    {
        if (NIdTipo == (int)TipoVerificacaoTokenEnum.Ativacao_Email)
            return Token;

        string chars = "abcdefghjkmnpqrstuvwxyz023456789ABCDEFGHIJLMNOPQRSTUVXZWYK!@#$";
        string pass = "";
        Random random = new Random();
        for (int f = 0; f < QuantidadeCaracter; f++)
        {
            pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
        }

        Token = BC.HashPassword(pass);
        
        
        return pass;
    }


    public enum TipoVerificacaoTokenEnum
    {
        Ativacao_Email = 5,
        Alterar_Senha = 6
    }
}