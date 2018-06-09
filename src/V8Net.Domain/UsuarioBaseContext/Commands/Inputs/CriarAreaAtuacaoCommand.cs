using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarAreaAtuacaoCommand : Notifiable, ICommand
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(Titulo, 20, "Titulo", "O campo título deve conter no máximo 20 caracteres")
                .HasMinLen(Titulo, 3, "Titulo", "O campo título deve conter no mínimo 3 caracteres")
                .HasMaxLen(Descricao, 200, "Descricao", "O campo descrição deve conter no máximo 20 caracteres")
                .HasMinLen(Descricao, 5, "Descricao", "O campo descrição deve conter no mínimo 5 caracteres")
            );
            return Valid;
        }
    }
}
