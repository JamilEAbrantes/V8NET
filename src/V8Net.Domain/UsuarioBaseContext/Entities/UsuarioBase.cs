using System;
using System.Collections.Generic;
using System.Linq;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Domain.UsuarioBaseContext.ValueObjects;
using V8Net.Shared.Entities;
using V8Net.Shared.Enums;

namespace V8Net.Domain.UsuarioBaseContext.Entities
{
    public class UsuarioBase : Entity
    {
        private readonly IList<Empresa> _empresas;
        private readonly IList<AreaAtuacao> _areasAtuacao;
        private readonly IList<Tela> _telas;

        public UsuarioBase()
        {
            _empresas = new List<Empresa>();
            _areasAtuacao = new List<AreaAtuacao>();
            _telas = new List<Tela>();
        }

        public UsuarioBase(
            LoginVO login,
            EmailVO email, 
            DocumentoVO documento,
            EPerfilAcessoSistema perfilAcessoSistema, 
            string impressoraZebra)
        {
            Login = login;
            Email = email;
            Documento = documento;
            ChaveDeAcesso = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            DataCadastro = DateTime.Now.Date;
            Ativo = EBoolean.True;
            PerfilAcesso = perfilAcessoSistema;
            ImpressoraZebra = impressoraZebra;

            _empresas = new List<Empresa>();
            _areasAtuacao = new List<AreaAtuacao>();
            _telas = new List<Tela>();

            AddNotifications(login, email, documento);
        }

        public LoginVO Login { get; private set; }
        public EmailVO Email { get; private set; }
        public DocumentoVO Documento { get; private set; }
        public string ChaveDeAcesso { get; private set; }        
        public DateTime DataCadastro { get; private set; }
        public EBoolean Ativo { get; private set; }
        public EPerfilAcessoSistema PerfilAcesso { get; private set; }
        public string ImpressoraZebra { get; private set; }
        public IReadOnlyCollection<Empresa> Empresas => _empresas.ToArray();
        public IReadOnlyCollection<AreaAtuacao> AreasAtuacao => _areasAtuacao.ToArray();
        public IReadOnlyCollection<Tela> Telas => _telas.ToArray();

        public bool Autenticar(string usuario, string senha)
        {
            if (Login.Usuario == usuario && Login.Senha == Login.EncriptarSenha(senha))
                return true;

            AddNotification("Usuario", "Usuário ou senha inválidos");
            return false;
        }        
        
        public void AtribuirUsuario(
            LoginVO loginVO, 
            EmailVO emailVO, 
            DocumentoVO documentoVO)
        {
            this.Login = loginVO;
            this.Email = emailVO;
            this.Documento = documentoVO;
        }

        public void AtribuirUsuario(
            LoginVO loginVO, 
            EmailVO emailVO, 
            DocumentoVO documentoVO, 
            EPerfilAcessoSistema perfilAcesso, 
            string impressoraZebra)
        {
            this.Login = loginVO;
            this.Email = emailVO;
            this.Documento = documentoVO;
            this.PerfilAcesso = perfilAcesso;
            this.ImpressoraZebra = impressoraZebra;
        }

        public void AtribuirUsuarioEmpresas(IEnumerable<Empresa> empresas)
        {
            var empresa = empresas.ToList();
            empresa.ForEach(x => _empresas.Add(x));
        }

        public void AtribuirUsuarioAtuacoes(IEnumerable<AreaAtuacao> areasAtuacao)
        {
            var atuacao = areasAtuacao.ToList();
            atuacao.ForEach(x => _areasAtuacao.Add(x));
        }

        public void AtribuirUsuarioTelas(IEnumerable<Tela> telas)
        {
            var tela = telas.ToList();
            tela.ForEach(x => _telas.Add(x));
        }

        public void Ativar() => Ativo = EBoolean.True;

        public void Desativar() => Ativo = EBoolean.False;   
        
        public void AlterarChaveDeAcesso() => ChaveDeAcesso = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        public override string ToString() => $"[  { GetType().Name } - Id: { Id }, Usuário: { Login.Usuario } ]";
    }
}
