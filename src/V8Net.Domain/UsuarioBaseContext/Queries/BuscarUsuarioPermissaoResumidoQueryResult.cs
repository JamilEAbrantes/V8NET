using System;
using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarUsuarioPermissaoResumidoQueryResult
    {
        public int IdPermissao { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string AreaAtuacao { get; set; }
        public int IdTela { get; set; }
        public string Tela { get; set; }
        public EBoolean Incluir { get; set; }
        public EBoolean Atualizar { get; set; }
        public EBoolean Excluir { get; set; }
        public EBoolean Consultar { get; set; }
    }
}
