using System;
namespace MarketList_Model
{
    public class AgrupadorListasDTO
    {
        public int UsuarioId { get; set; }
        public int[] ListaIds { get; set; }
        public int Status { get; set; }
        public string Descricao { get; set; }
    }
}