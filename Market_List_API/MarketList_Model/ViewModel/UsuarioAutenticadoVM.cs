using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public class UsuarioAutenticadoVM
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TipoId { get; set; }
        public int StatusId { get; set; }
        public bool SenhaTemporaria { get; set; }
    }
}