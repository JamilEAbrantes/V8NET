using System;
using FluentValidator.Validation;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Entities;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class Tela : Entity
    {
        public Tela() { }

        public Tela(AreaAtuacao areaAtuacao, string titulo, string descricao, string link)
        {
            AreaAtuacao = areaAtuacao;
            Titulo = titulo;
            Descricao = descricao;
            Link = link;
            DataCadastro = DateTime.Now;
            Ativo = EBoolean.True;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsNotNull(AreaAtuacao, "AreaAtuacao", "A tela deve pertencer à uma área de atuação")
                .HasMaxLen(Titulo, 30, "Titulo", "O campo título deve conter no máximo 20 caracteres")
                .HasMinLen(Titulo, 3, "Titulo", "O campo título deve conter no mínimo 3 caracteres")
                .HasMaxLen(Descricao, 200, "Descricao", "O campo descrição deve conter no máximo 20 caracteres")
                .HasMinLen(Descricao, 5, "Descricao", "O campo descrição deve conter no mínimo 5 caracteres")
                .HasMaxLen(Link, 200, "Link", "O campo link deve conter no máximo 20 caracteres")
                .HasMinLen(Link, 5, "Link", "O campo link deve conter no mínimo 5 caracteres")
            );
        }

        public AreaAtuacao AreaAtuacao { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string Link { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public EBoolean Ativo { get; private set; }

        public void AtribuirTela(string titulo, string descricao, string link)
        {
            this.Titulo = titulo;
            this.Descricao = descricao;
            this.Link = link;
        }

        public void AtribuirAreaAtuacao(AreaAtuacao areaAtuacao) => this.AreaAtuacao = areaAtuacao;

        public void Ativar() => this.Ativo = EBoolean.True;

        public void Desativar() => this.Ativo = EBoolean.False;

        public override string ToString() =>  $"[ { GetType().Name } - Id: { Id }, Título: { Titulo }, Área: { AreaAtuacao.Id } - { AreaAtuacao.Titulo } ]";
    }
}
