using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class ExcluirAreaAtuacaoCommand : Notifiable, ICommand
    {
        public int Id { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Id, 0, "Id", "Informe um código de atuação válido")
            );
            return Valid;
        }
    }
}
