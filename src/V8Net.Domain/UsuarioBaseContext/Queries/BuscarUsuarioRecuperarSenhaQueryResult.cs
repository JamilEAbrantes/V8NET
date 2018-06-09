using System;
using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarUsuarioSenhaTemporariaQueryResult
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string SenhaTemporaria { get; set; }
        public DateTime DataCadastro { get; set; }
        public EBoolean Ativo { get; set; }
    }
}
