using V8Net.Domain.UsuarioBaseContext.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarTelaResumidoQueryResult
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string AreaAtuacao { get; set; }
        public EBoolean Ativo { get; set; }
    }
}
