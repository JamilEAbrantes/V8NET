using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class AutenticarUsuarioBaseCommand : Notifiable, ICommand
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public bool IsValidCommand() // Re-hidratando as validações antes de dar hit no banco (failsfast validation)
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .HasMinLen(Usuario, 3, "Usuario", "O usuário deve conter pelo menos 3 caracteres")
                .HasMaxLen(Usuario, 20, "Usuario", "O usuário deve conter no máximo 20 caracteres")
                .HasMaxLen(Senha, 20, "Senha", "O campo senha deve conter no máximo 20 caracteres")
                .HasMinLen(Senha, 5, "Senha", "O campo senha deve conter no mínimo 5 caracteres")
            );
            return Valid;
        }
    }
}
