using Microsoft.VisualStudio.TestTools.UnitTesting;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Shared.Enums;

namespace V8Net.Tests.Commands
{
    [TestClass]
    public class CriarUsuarioBaseCommandTeste
    {
        [TestMethod]
        public void Inicia_CriarUsuarioBaseCommand_VALIDA()
        {
            var command = new CriarUsuarioBaseCommand();
            command.Usuario = "Jamil";
            command.Senha = "jaja123";
            command.Email = "jamilabrantes.dev@outlook.com";
            command.Documento = "53748389469";
            command.PerfilAcesso = EPerfilAcessoSistema.Administrador;
            command.ImpressoraZebra = @"C:\imp";

            Assert.AreEqual(true, command.IsValidCommand());
        }

        [TestMethod]
        public void Inicia_CriarUsuarioBaseCommand_INVALIDA()
        {
            var command = new CriarUsuarioBaseCommand();
            command.Usuario = "Jamil";
            command.Senha = "jaja123";
            command.Email = "jamilabrantes.dev@outlook.com";
            command.Documento = "537483894"; // <- Documento inválido
            command.PerfilAcesso = EPerfilAcessoSistema.Administrador;
            command.ImpressoraZebra = @"C:\imp";

            Assert.AreEqual(false, command.IsValidCommand());
        }
    }
}
