using System;

namespace MarketList_Model
{
    public class UsuarioAutenticadoVM
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int TipoId { get; set; }

        public int? UnidadeId { get; set; }

        public string NomeUnidade { get; set; }
    }
}