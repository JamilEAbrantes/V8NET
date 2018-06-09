using System.Collections.Generic;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;

namespace V8Net.Domain.UsuarioBaseContext.Repositories
{
    public interface IAreaAtuacaoRepository
    {
        AreaAtuacao AreaAtuacao(int id);
        EditarAreaAtuacaoQueryResult Editar(int id);
        BuscarAreaAtuacaoResumidoQueryResult BuscarPorId(int id);
        IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarPorNome(string nome);
        IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarTodos();
        void Salvar(AreaAtuacao areaAtuacao);
        void Atualizar(AreaAtuacao areaAtuacao);
        void Excluir(int id);
        bool AreaAtuacaoExistente(string nome);
    }
}
