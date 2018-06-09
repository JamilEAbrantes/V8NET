using FluentValidator.Validation;
using V8Net.Shared.Entities;
using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class Empresa : Entity
    {
        public Empresa() { }

        public Empresa(
            int id,
            string nome, 
            string fantasia, 
            string telefone, 
            ETipoEmpresa tipoEmpresa, 
            int cgc9, 
            int cgc4, 
            int cgc2)
        {
            Id = id;
            Nome = nome;
            Fantasia = fantasia;
            Telefone = telefone;
            TipoEmpresa = tipoEmpresa;
            Cgc9 = cgc9;
            Cgc4 = cgc4;
            Cgc2 = cgc2;

            AddNotifications(new ValidationContract()
                .Requires()
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
        }
        
        public string Nome { get; private set; }
        public string Fantasia { get; private set; }
        public string Telefone { get; private set; }
        public ETipoEmpresa TipoEmpresa { get; private set; }
        public int Cgc9 { get; private set; }
        public int Cgc4 { get; private set; }
        public int Cgc2 { get; private set; }

        public void AtribuirEmpresa(string nome, string fantasia, string telefone,
            ETipoEmpresa tipoEmpresa, int cgc9, int cgc4, int cgc2)
        {
            this.Nome = nome;
            this.Fantasia = fantasia;
            this.Telefone = telefone;
            this.TipoEmpresa = tipoEmpresa;
            this.Cgc9 = cgc9;
            this.Cgc4 = cgc4;
            this.Cgc2 = cgc2;
        }

        public override string ToString() => $"[ { GetType().Name } - Id: { Id }, N. Fantasia: { Fantasia }, Doc.: { Cgc9 }/{ Cgc4 }-{ Cgc2 } ]";
    }
}
