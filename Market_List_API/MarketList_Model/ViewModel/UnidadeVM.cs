using System.Collections.Generic;
using MarketList_Model;

public class UnidadeVM
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<ListaVM> Listas { get; set; }
}