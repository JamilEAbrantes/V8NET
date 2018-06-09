namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class BuscarUsuariosBaseResumidoQueryResult
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public string Ativo { get; set; }
    }
}
