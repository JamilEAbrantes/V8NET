using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class AtualizarUsuarioBaseCommand : Notifiable, ICommand
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public EBoolean Ativo { get; set; }
        public EPerfilAcessoSistema PerfilAcesso { get; set; }
        public string ImpressoraZebra { get; set; }

        public bool IsValidCommand() // Re-hidratando as validações antes de dar hit no banco (failsfast validation)
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Id, 0, "Id", "Informe um código de usuário válido")
                .HasMinLen(Usuario, 3, "Usuario", "O usuário deve conter pelo menos 3 caracteres")
                .HasMaxLen(Usuario, 20, "Usuario", "O usuário deve conter no máximo 20 caracteres")
                .HasMaxLen(Email, 50, "Email", "O Email deve conter no máximo 50 caracteres")
                .HasLen(Documento, 14, "Documento", "Número de documento inválido")
                .HasMaxLen(ImpressoraZebra, 200, "ImpressoraZebra", "O campo impressora deve conter no máximo 200 caracteres")
            );
            return Valid;
        }
    }
}
