using Microsoft.VisualStudio.TestTools.UnitTesting;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Shared.Enums;

namespace V8Net.Tests.Entities
{
    [TestClass]
    public class EmpresaTest
    {
        [TestMethod]
        public void Inicia_construtor_Valido()
        {
            var empresa = new Empresa(1,"JEA Enterprise", "JEA", "12345678", ETipoEmpresa.Filial, 123456789, 1234, 12);
            Assert.AreEqual(true, empresa.Valid);
        }

        [TestMethod]
        public void Inicia_construtor_Invalido()
        {
            var empresa = new Empresa(1, "JEA Enterprise", "JEA", "12345678", ETipoEmpresa.Filial, 1234567, 123, 1);
            Assert.AreEqual(true, empresa.Invalid);
        }
    }
}
