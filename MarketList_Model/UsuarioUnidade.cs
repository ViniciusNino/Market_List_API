
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketList_Model;
public partial class UsuarioUnidade : Entity<UsuarioUnidade>
{
    public UsuarioUnidade(){}
    public UsuarioUnidade(int idUnidade, int idUsuario)
    {
        NIdUnidade = idUnidade;
        NIdUsuario = idUsuario;
    }
    public int NIdUsuario { get; private set; }
    public int NIdUnidade { get; private set; }

    [NotMapped]
    public virtual Usuario Usuario { get; set; }
    [NotMapped]
    public virtual Unidade Unidade { get; set; }
    public override bool IsValid() => true;
}

