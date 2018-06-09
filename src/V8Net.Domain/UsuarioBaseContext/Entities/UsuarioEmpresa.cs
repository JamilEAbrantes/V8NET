using FluentValidator.Validation;
using V8Net.Shared.Entities;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class UsuarioEmpresa : Entity
    {
        public UsuarioEmpresa() { }

        public UsuarioEmpresa(int idUsuario, int idEmpresa)
        {
            IdUsuario = idUsuario;
            IdEmpresa = idEmpresa;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(IdUsuario, 0, "IdUsuario", "Informe um usuário válido")
                .IsGreaterThan(IdEmpresa, 0, "IdEmpresa", "Informe uma empresa válida")
            );
        }

        public int IdUsuario { get; private set; }
        public int IdEmpresa { get; private set; }

        public override string ToString() => $"[ { GetType().Name } - Id: { Id }, Usuário: { IdUsuario }, Empresa: { IdUsuario } ]";
    }
}
