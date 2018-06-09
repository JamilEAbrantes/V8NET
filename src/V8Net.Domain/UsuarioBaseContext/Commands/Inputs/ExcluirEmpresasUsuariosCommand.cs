using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class ExcluirEmpresasUsuariosCommand : Notifiable, ICommand
    {
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsLowerOrEqualsThan(0, IdEmpresa, "IdEmpresa", "Informe uma empresa")
                .IsLowerOrEqualsThan(0, IdUsuario, "IdUsuario", "Informe um usuário")
            );
            return Valid;
        }
    }
}
