using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarUsuarioAtuacaoCommand : Notifiable, ICommand
    {
        public int IdUsuario { get; set; }
        public int IdAreaAtuacao { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(IdUsuario, 0, "IdUsuario", "Informe um usuário válido")
                .IsGreaterThan(IdAreaAtuacao, 0, "IdAreaAtuacao", "Informe uma área de atuação válida")
            );
            return Valid;
        }
    }
}
