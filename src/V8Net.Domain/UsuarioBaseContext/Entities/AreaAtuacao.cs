using System;
using FluentValidator.Validation;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Entities;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class AreaAtuacao : Entity
    {
        public AreaAtuacao() { }

        public AreaAtuacao(string titulo, string descricao)
        {
            Titulo = titulo;
            Descricao = descricao;
            DataCadastro = DateTime.Now.Date; 
            Ativo = EBoolean.True;

            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(Titulo, 20, "Titulo", "O campo título deve conter no máximo 20 caracteres")
                .HasMinLen(Titulo, 3, "Titulo", "O campo título deve conter no mínimo 3 caracteres")
                .HasMaxLen(Descricao, 200, "Descricao", "O campo descrição deve conter no máximo 20 caracteres")
                .HasMinLen(Descricao, 5, "Descricao", "O campo descrição deve conter no mínimo 5 caracteres")
            );
        }

        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public EBoolean Ativo { get; private set; }

        public void AtribuirAreaAtuacao(string titulo, string descricao)
        {
            this.Titulo = titulo;
            this.Descricao = descricao;
        }

        public void Ativar() => this.Ativo = EBoolean.True;

        public void Desativar() => this.Ativo = EBoolean.False;

        public override string ToString() => $"[ { GetType().Name } - Id: { Id }, Área: { Titulo } ]";
    }
}
