using System;

namespace MarketList_Model
{
    public class ListaVM
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime? Cadastro { get; set; }
        public bool selecionado { get; set; }
    }
}