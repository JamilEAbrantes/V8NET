using FluentValidator.Validation;
using V8Net.Shared.Entities;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class UsuarioAreaAtuacao : Entity
    {
        public UsuarioAreaAtuacao() { }

        public UsuarioAreaAtuacao(UsuarioBase usuario, AreaAtuacao areaAtuacao)
        {
            Usuario = usuario;
            AreaAtuacao = areaAtuacao;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsNotNull(Usuario, "Usuario", "A área de atuação deve ter um funcionario")
                .IsNotNull(AreaAtuacao, "AreaAtuacao", "O usuário deve ter uma área de atuação")
            );
        }

        public UsuarioBase Usuario { get; private set; }
        public AreaAtuacao AreaAtuacao { get; private set; }

        public override string ToString() => $"[ { GetType().Name } - Id: { Id }, Usuário: { Usuario.Id } - { Usuario.Login.Usuario }, Área: { AreaAtuacao.Id } - { AreaAtuacao.Titulo } ]";
    }
}
