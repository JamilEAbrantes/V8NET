namespace V8Net.Domain.UsuarioBaseContext.Services
{
    public interface IEmailService
    {
        void RecuperarSenha(string email, string senha);
    }
}
