using System.Collections.Generic;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;

namespace V8Net.Domain.UsuarioBaseContext.Repositories
{
    public interface IEmpresaRepository
    {
        Empresa Empresa(int id);
        Empresa Empresa(string nome);
        EditarEmpresaQueryResult Editar(int id);
        BuscarEmpresasResumidoQueryResult BuscarPorId(int id);
        IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarTodos();
        IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarPorNome(string nome);
        void Salvar(Empresa empresa);
        void Atualizar(Empresa empresa);
        bool EmpresaExistente(int id);
        bool EmpresaExistente(string nome);
        void Excluir(int id);
    }
}
