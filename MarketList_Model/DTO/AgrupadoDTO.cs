using System;
using System.Collections.Generic;

namespace MarketList_Model
{
    public class AgrupadorListasDTO
    {
        public int UsuarioId { get; set; }
        public List<int> ListaIds { get; set; }
        public int Status { get; set; }
        public string Descricao { get; set; }
    }
}