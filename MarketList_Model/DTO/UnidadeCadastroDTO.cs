using System;

namespace MarketList_Model;
public class UnidadeCadastroDTO
{
    public int IdUsuario { get; set; }
    public string Nome { get; set; }

    public Unidade GetUnidade() => new (Nome);
    public UsuarioUnidade GetUsuarioUnidade(int idUnidade) => new(idUnidade, IdUsuario);
}