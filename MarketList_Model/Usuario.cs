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
            Tipo = new Tipo();
            Status = new Status();
            ItensListaComprador = new HashSet<ItemLista>();
            ItensListaSolicitante = new HashSet<ItemLista>();
            Listas = new HashSet<Lista>();
            Agrupadores = new HashSet<Agrupador>();
        }

        public Usuario(string nome, string senha, string email, int tipoId = (int)PerfitlUsuarioEnum.Administrador, int status = (int)StatusUsuarioEnum.Aguardando_Ativacao_Email)
        {
            SNome = nome;
            SSenha = senha;
            SEmail = email;
            NIdTipo = tipoId;
            NIdStatus = status;
        }
        public string SUsuario { get; private set; }
        public string SNome { get; set; }
        public string SSenha { get; private set; }
        public string SEmail { get; private set; }
        public int NIdTipo { get; set; }
        public int NIdStatus { get; set; }
        public DateTime DCadastro { get; set; }

        public virtual Tipo Tipo { get; set; }

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

    public enum PerfitlUsuarioEnum
    {
        root = 1,
        Administrador = 2,
        Comprador = 3,
        Solicitante = 4,
    }
}
