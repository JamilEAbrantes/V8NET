using System;
using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class EditarTelaQueryResult
    {
        public int Id { get; set; }
        public int IdAreaAtuacao { get; set; }
        public string AreaAtuacaoTitulo { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public DateTime DataCadastro { get; set; }
        public EBoolean Ativo { get; set; }
    }
}
