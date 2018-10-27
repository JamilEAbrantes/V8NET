using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Infra.Data.DataContext;

namespace V8Net.Infra.Data.UsuarioBase.Repositories
{
    public class AreaAtuacaoRepository : IAreaAtuacaoRepository
    {
        private readonly V8NetDataContext _context;

        public AreaAtuacaoRepository(V8NetDataContext context)
        {
            _context = context;
        }

        public AreaAtuacao AreaAtuacao(int id)
        {
            var query = @"SELECT Id, Titulo, Descricao, DataCadastro, Ativo FROM AreaAtuacao WHERE Id = :Id";

            var areaAtuacao = _context
                .Connection
                .Query<AreaAtuacao>(query, new { Id = id });

            return areaAtuacao.FirstOrDefault();
        }

        public EditarAreaAtuacaoQueryResult Editar(int id)
        {
            var areaAtuacao = AreaAtuacao(id);
            if (areaAtuacao == null)
                return null;

            return new EditarAreaAtuacaoQueryResult()
            {
                Id = areaAtuacao.Id,
                Titulo = areaAtuacao.Titulo,
                Descricao = areaAtuacao.Descricao,
                DataCadastro = areaAtuacao.DataCadastro,
                Ativo = areaAtuacao.Ativo
            };
        }

        public BuscarAreaAtuacaoResumidoQueryResult BuscarPorId(int id)
        {
            var query = @"SELECT Id, Titulo, Descricao, Ativo FROM AreaAtuacao WHERE Id = :Id";

            var areaAtuacao = _context
                .Connection
                .Query<BuscarAreaAtuacaoResumidoQueryResult>(query, new { Id = id });

            return areaAtuacao.FirstOrDefault();
        }

        public IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarPorNome(string nome)
        {
            var query = @"SELECT Id, Titulo, Descricao, Ativo FROM AreaAtuacao WHERE Titulo LIKE '%" + nome + "%' ORDER BY Id desc";

            var areaAtuacao = _context
                .Connection
                .Query<BuscarAreaAtuacaoResumidoQueryResult>(query, new { });

            return areaAtuacao;
        }

        public IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarTodos()
        {
            var query = @"SELECT Id, Titulo, Descricao, Ativo FROM AreaAtuacao ORDER BY Id desc";

            var areaAtuacao = _context
                .Connection
                .Query<BuscarAreaAtuacaoResumidoQueryResult>(query, new { });

            return areaAtuacao;
        }

        public void Salvar(AreaAtuacao areaAtuacao)
        {
            var query = new StringBuilder();
            query.Append("INSERT INTO AreaAtuacao \n");
            query.Append("(Titulo, Descricao, DataCadastro, Ativo) \n");
            query.Append("VALUES \n");
            query.Append("(:Titulo, :Descricao, :DataCadastro, :Ativo) returning Id into :Id");

            var param = new DynamicParameters();
            param.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add(name: "Titulo", value: areaAtuacao.Titulo, direction: ParameterDirection.Input);
            param.Add(name: "Descricao", value: areaAtuacao.Descricao, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: areaAtuacao.DataCadastro, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)areaAtuacao.Ativo, direction: ParameterDirection.Input);

            _context.Connection.Execute(query.ToString(), param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public void Atualizar(AreaAtuacao areaAtuacao)
        {
            var query = new StringBuilder();
            query.Append("UPDATE AreaAtuacao SET \n");
            query.Append("Titulo = :Titulo, Descricao = :Descricao, Ativo = :Ativo \n");
            query.Append("WHERE Id = :Id");

            var param = new DynamicParameters();
            param.Add(name: "Id", value: areaAtuacao.Id, direction: ParameterDirection.Input);
            param.Add(name: "Titulo", value: areaAtuacao.Titulo, direction: ParameterDirection.Input);
            param.Add(name: "Descricao", value: areaAtuacao.Descricao, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)areaAtuacao.Ativo, direction: ParameterDirection.Input);

            _context.Connection.Execute(query.ToString(), param);
        }

        public void Excluir(int id)
        {
            var query = @"DELETE FROM AreaAtuacao WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        public bool AreaAtuacaoExistente(string nome)
        {
            var query = @"SELECT atuacao.* FROM AreaAtuacao atuacao WHERE Titulo = :Nome";

            var result = _context
                .Connection
                .Query<bool>(query, new { Nome = nome });

            return result.Any();
        }
    }
}
