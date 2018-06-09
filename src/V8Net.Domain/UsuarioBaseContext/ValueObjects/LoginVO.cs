using System;
using FluentValidator;
using FluentValidator.Validation;
using System.Text;

namespace V8Net.Domain.UsuarioBaseContext.ValueObjects
{
    public class LoginVO : Notifiable
    {
        public LoginVO() { }

        public LoginVO(string usuario, string senha)
        {
            Usuario = usuario;
            Senha = EncriptarSenha(senha);

            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(Usuario, 20, "Usuario", "O campo usuario deve conter no máximo 20 caracteres")
                .HasMinLen(Usuario, 3, "Usuario", "O campo usuario deve conter no mínimo 3 caracteres")
            );
        }

        public LoginVO(string usuario)
        {
            Usuario = usuario;

            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(Usuario, 20, "Usuario", "O campo usuario deve conter no máximo 20 caracteres")
                .HasMinLen(Usuario, 3, "Usuario", "O campo usuario deve conter no mínimo 3 caracteres")
            );
        }

        public string Usuario { get; private set; }
        public string Senha { get; private set; }

        public string EncriptarSenha(string pass)
        {
            if (string.IsNullOrEmpty(pass)) return "";
            var password = (pass += "|08271c47-7058-4231-884c-ed1d8eb00229");
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString();
        }

        public void AtribuirSenha(string senha) => this.Senha = senha;

        public string GerarSenha() => Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();

        public override string ToString() => $"[ { GetType().Name } - Usuário: { Usuario }, Senha: { Senha } ]";
    }
}
