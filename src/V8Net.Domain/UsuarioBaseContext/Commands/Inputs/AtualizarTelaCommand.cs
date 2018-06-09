using FluentValidator;
using FluentValidator.Validation;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class AtualizarTelaCommand : Notifiable, ICommand
    {
        public int Id { get; set; }
        public int IdAreaAtuacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public EBoolean Ativo { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Id, 0, "IdTela", "Informe um código de tela válido")
                .IsGreaterThan(IdAreaAtuacao, 0, "IdAreaAtuacao", "Informe uma area de atuação válida")
                .HasMaxLen(Titulo, 30, "Titulo", "O campo título deve conter no máximo 20 caracteres")
                .HasMinLen(Titulo, 3, "Titulo", "O campo título deve conter no mínimo 3 caracteres")
                .HasMaxLen(Descricao, 200, "Descricao", "O campo descrição deve conter no máximo 20 caracteres")
                .HasMinLen(Descricao, 5, "Descricao", "O campo descrição deve conter no mínimo 5 caracteres")
                .HasMaxLen(Link, 200, "Link", "O campo link deve conter no máximo 20 caracteres")
                .HasMinLen(Link, 5, "Link", "O campo link deve conter no mínimo 5 caracteres")
            );
            return Valid;
        }
    }
}
