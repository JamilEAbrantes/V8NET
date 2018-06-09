using System.Collections.Generic;
using FluentValidator;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Inputs
{
    public class CriarListaEmpresasUsuariosCommand : Notifiable, ICommand, ICriarListaEmpresasUsuarios<UsuarioEmpresa>
    {
        public IEnumerable<UsuarioEmpresa> EmpresaUsuario { get; set; }

        public bool IsValidCommand()
        {
            return Valid;
        }
    }

    public interface ICriarListaEmpresasUsuarios<T>
    {
        IEnumerable<T> EmpresaUsuario { get; set; }
    }
}
