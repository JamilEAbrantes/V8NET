using System;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Queries
{
    public class EditarUsuariosBaseQueryResult
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public string ChaveDeAcesso { get; set; }
        public DateTime DataCadastro { get; set; }
        public EBoolean Ativo { get; set; }
        public EPerfilAcessoSistema PerfilAcesso { get; set; }
        public string ImpressoraZebra { get; set; }
    }
}
