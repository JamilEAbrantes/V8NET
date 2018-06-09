using System;
using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class EditarAreaAtuacaoQueryResult
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public EBoolean Ativo { get; set; }
    }
}
