using Microsoft.VisualStudio.TestTools.UnitTesting;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.ValueObjects;
using V8Net.Shared.Enums;

namespace V8Net.Tests.Entities
{
    [TestClass]
    public class UsuarioBaseTeste
    {
        private DocumentoVO _documentoVO;
        private EmailVO _emailVO;
        private LoginVO _loginVO;
        private UsuarioBase _usuarioBase;

        public UsuarioBaseTeste()
        {
            _documentoVO = new DocumentoVO("181.091.714-00");
            _emailVO = new EmailVO("jamilabrantes.dev@outlook.com");
            _loginVO = new LoginVO("jamil", "jaja123");
            _usuarioBase = new UsuarioBase(_loginVO, _emailVO, _documentoVO, EPerfilAcessoSistema.Usuario, @"C:\impr");
        }

        [TestMethod]
        public void Inicia_DocumentoVO_VALIDO()
        {
            Assert.AreEqual(true, _documentoVO.Valid);
        }

        [TestMethod]
        public void Inicia_DocumentoVO_INVALIDO()
        {
            _documentoVO = new DocumentoVO("181.091.714-0x");
            Assert.AreEqual(true, _documentoVO.Invalid);
        }

        [TestMethod]
        public void Inicia_EmailVO_VALIDO()
        {
            Assert.AreEqual(true, _emailVO.Valid);
        }

        [TestMethod]
        public void Inicia_EmailVO_INVALIDO()
        {
            _emailVO = new EmailVO("jamilabrantes@.com");
            Assert.AreEqual(true, _emailVO.Invalid);
        }

        [TestMethod]
        public void Inicia_LoginVO_VALIDO()
        {
            Assert.AreEqual(true, _loginVO.Valid);
        }

        [TestMethod]
        public void Inicia_LoginVO_INVALIDO()
        {
            _loginVO = new LoginVO("ja", "ja1000"); 
            Assert.AreEqual(true, _loginVO.Invalid);
        }

        [TestMethod]
        public void Inicia_UsuarioBaseVO_VALIDO()
        {
            Assert.AreEqual(true, _usuarioBase.Valid);
        }

        [TestMethod]
        public void Inicia_UsuarioBaseVO_INVALIDO()
        {
            _documentoVO = new DocumentoVO("181.091.714-00");
            _emailVO = new EmailVO("jamilabrantes.dev@outlook.com");
            _loginVO = new LoginVO("ja", "jaja"); // <- Usuário inválido
            _usuarioBase = new UsuarioBase(_loginVO, _emailVO, _documentoVO, EPerfilAcessoSistema.Usuario, @"C:\impr");
            
            Assert.AreEqual(true, _usuarioBase.Invalid);
        }

    }
}
