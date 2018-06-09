using FluentValidator;
using FluentValidator.Validation;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarUsuarioTelaCommand : Notifiable, ICommand
    {
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
                .IsGreaterThan(IdUsuario, 0, "IdUsuarioBase", "Informe um usuário válido")
                .IsGreaterThan(IdTela, 0, "IdTela", "Informe uma tela válida")
            );
            return Valid;
        }
    }
}
