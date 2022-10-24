using System.Collections.Generic;

namespace MarketList_Model
{
    public class UsuarioAutenticadoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public int TipoId { get; set; }
        public string Senha { get; set; }
        public string SenhaToken { get; set; }
        public bool SenhaTemporariaExpirada { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool SenhaTemporaria { get; set; }
    }
}