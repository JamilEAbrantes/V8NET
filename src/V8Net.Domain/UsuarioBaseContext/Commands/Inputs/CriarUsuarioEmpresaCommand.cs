using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarUsuarioEmpresaCommand : Notifiable, ICommand
    {
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(IdUsuario, 0, "IdUsuario", "Informe um usuário válido")
                .IsGreaterThan(IdEmpresa, 0 , "IdEmpresa", "Informe uma empresa válida")
            );
            return Valid;
        }
    }
}
