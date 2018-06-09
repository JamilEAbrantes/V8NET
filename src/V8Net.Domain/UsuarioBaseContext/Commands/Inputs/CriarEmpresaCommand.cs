using FluentValidator;
using FluentValidator.Validation;
using V8Net.Shared.Commands;
using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarEmpresaCommand : Notifiable, ICommand
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; }
        public string Telefone { get; set; }
        public ETipoEmpresa TipoEmpresa { get; set; }
        public int Cgc9 { get; set; }
        public int Cgc4 { get; set; }
        public int Cgc2 { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Id, 0, "Id", "Informe códdigo de empresa válido")
                .HasMaxLen(Nome, 35, "Nome", "O campo nome deve conter no máximo 35 caracteres")
                .HasMinLen(Nome, 3, "Nome", "O campo título deve conter no mínimo 3 caracteres")
                .HasMaxLen(Fantasia, 20, "Fantasia", "O campo fantasia deve conter no máximo 20 caracteres")
                .HasMinLen(Fantasia, 3, "Fantasia", "O campo fantasia deve conter no mínimo 3 caracteres")
                .HasMaxLen(Telefone, 8, "Telefone", "O campo telefone deve conter 8 caracteres")
                .HasMinLen(Telefone, 8, "Telefone", "O campo telefone deve conter 8 caracteres")
                .HasMaxLen(Cgc9.ToString(), 9, "Cgc9", "O campo cgc9 deve conter 9 caracteres")
                .HasMinLen(Cgc9.ToString(), 9, "Cgc9", "O campo cgc9 deve conter 9 caracteres")
                .HasMaxLen(Cgc4.ToString(), 4, "Cgc4", "O campo cgc4 deve conter 4 caracteres")
                .HasMinLen(Cgc4.ToString(), 4, "Cgc4", "O campo cgc4 deve conter 4 caracteres")
                .HasMaxLen(Cgc2.ToString(), 2, "Cgc2", "O campo cgc2 deve conter 2 caracteres")
                .HasMinLen(Cgc2.ToString(), 2, "Cgc2", "O campo cgc2 deve conter 2 caracteres")
            );
            return Valid;
        }
    }
}
