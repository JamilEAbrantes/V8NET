using FluentValidator.Validation;
using System;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Entities;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class UsuarioTela : Entity
    {
        public UsuarioTela() { }

        public UsuarioTela(UsuarioBase usuarioBase, Tela tela)
        {
            UsuarioBase = usuarioBase;
            Tela = tela;
            Incluir = EBoolean.True;
            Atualizar = EBoolean.True;
            Excluir = EBoolean.True;
            Consultar = EBoolean.True;
            DataCadatro = DateTime.Now;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsNotNull(UsuarioBase, "UsuarioBase", "Informe um usuário válido")
                .IsNotNull(Tela, "Tela", "Informe uma tela válida")
            );
        }

        public UsuarioBase UsuarioBase { get; private set; }
        public Tela Tela { get; private set; }
        public EBoolean Incluir { get; private set; }
        public EBoolean Atualizar { get; private set; }
        public EBoolean Excluir { get; private set; }
        public EBoolean Consultar { get; private set; }
        public DateTime DataCadatro { get; private set; }

        public void AtribuirUsuario(UsuarioBase usuarioBase) => this.UsuarioBase = usuarioBase;

        public void AtribuirTela(Tela tela) => this.Tela = tela;

        public void AtribuirUsuarioPemissoes(EBoolean incluir, EBoolean atualizar, EBoolean excluir, EBoolean consultar)
        {
            this.Incluir = incluir;
            this.Atualizar = atualizar;
            this.Excluir = excluir;
            this.Consultar = consultar;
        }

        public override string ToString() => $"[ { GetType().Name } - Id: { Id }, Usuário: { UsuarioBase.Id } - { UsuarioBase.Login.Usuario }, Tela: { Tela.Id } - { Tela.Titulo } ]";
    }
}
