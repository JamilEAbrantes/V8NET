using FluentValidator;
using FluentValidator.Validation;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class AtualizarUsuarioPermissaoCommand : Notifiable, ICommand
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdTela { get; set; }
        public EBoolean Incluir { get; set; }
        public EBoolean Atualizar { get; set; }
        public EBoolean Excluir { get; set; }
        public EBoolean Consultar { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Id, 0, "Id", "Informe um id válido")
                .IsGreaterThan(IdUsuario, 0, "IdUsuario", "Informe um usuário válido")
                .IsGreaterThan(IdTela, 0, "IdTela", "Informe uma tela válida")
            );
            return Valid;
        }
    }
}
