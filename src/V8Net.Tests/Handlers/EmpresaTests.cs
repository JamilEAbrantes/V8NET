using Microsoft.VisualStudio.TestTools.UnitTesting;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;

namespace V8Net.Tests.Handlers
{
    [TestClass]
    public class EmpresaTests
    {
        [TestMethod]
        public void Criar_Empresa_Command_VALIDO()
        {
            var command = new CriarUsuarioEmpresaCommand();
            command.IdEmpresa = 1;
            command.IdUsuario = 1;

            Assert.AreEqual(true, command.IsValidCommand());
        }

        [TestMethod]
        public void Criar_Empresa_Command_INVALIDO()
        {
            var command = new CriarUsuarioEmpresaCommand();
            command.IdEmpresa = 0;
            command.IdUsuario = 0;

            Assert.AreEqual(false, command.IsValidCommand());
        }
    }
}
