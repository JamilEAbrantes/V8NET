using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class RecuperarSenhaCommand : Notifiable, ICommand
    {
        public string Email { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsEmail(Email, "Email", "Informe um e-mail válido")
            );
            return Valid;
        }
    }
}
