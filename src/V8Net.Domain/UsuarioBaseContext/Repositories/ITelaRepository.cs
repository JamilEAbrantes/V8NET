using System.Collections.Generic;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;

namespace V8Net.Domain.UsuarioBaseContext.Repositories
{
    public interface ITelaRepository
    {
        Tela Tela(int id);
        EditarTelaQueryResult Editar(int id);
        BuscarTelaResumidoQueryResult BuscarPorId(int id);
        IEnumerable<BuscarTelaResumidoQueryResult> BuscarPorNome(string nome);
        IEnumerable<BuscarTelaResumidoQueryResult> BuscarTodos();
        void Salvar(Tela areaAtuacao);
        void Atualizar(Tela areaAtuacao);
        void Excluir(int id);
        bool TelaExistente(int ind);
        bool TelaExistente(string nome);
    }
}
