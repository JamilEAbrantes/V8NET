using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarEmpresasResumidoQueryResult
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; }
        public string Telefone { get; set; }
        public ETipoEmpresa TipoEmpresa { get; set; }
        public int Cgc9 { get; set; }
        public int Cgc4 { get; set; }
        public int Cgc2 { get; set; }
    }
}
