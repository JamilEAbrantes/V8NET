using System;
using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class EditarUsuarioTelaQueryResult
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdTela { get; set; }
        public EBoolean Incluir { get; set; }
        public EBoolean Atualizar { get; set; }
        public EBoolean Excluir { get; set; }
        public EBoolean Consultar { get; set; }
        public DateTime DataCadatro { get; set; }
    }
}
