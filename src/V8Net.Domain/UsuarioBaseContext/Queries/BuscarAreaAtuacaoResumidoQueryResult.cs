using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarAreaAtuacaoResumidoQueryResult
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public EBoolean Ativo { get; set; }
    }
}
