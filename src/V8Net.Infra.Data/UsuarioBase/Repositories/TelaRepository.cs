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
    public class TelaRepository : ITelaRepository
    {
        private readonly V8NetDataContext _context;

        public TelaRepository(V8NetDataContext context)
        {
            _context = context;
        }

        public Tela Tela(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT        tela.Id, tela.idAreaAtuacao, tela.Titulo, tela.Descricao, tela.Link, tela.DataCadastro, tela.Ativo, \n");
            query.Append("				areaAtuacao.Id, areaAtuacao.Titulo, areaAtuacao.Descricao, areaAtuacao.DataCadastro, areaAtuacao.Ativo \n");
            query.Append("FROM 			TELAS tela \n");
            query.Append("INNER JOIN	AREAATUACAO areaAtuacao ON (tela.IdAreaAtuacao = areaAtuacao.Id) \n");
            query.Append("WHERE			tela.ID = :Id");

            var tela = _context
                .Connection
                .Query<Tela, AreaAtuacao, Tela>(query.ToString(),
                    (t, aa) =>
                    {
                        t.AtribuirAreaAtuacao(aa);
                        return t;
                    },
                    new { Id = id },
                    splitOn: "Id");

            return tela.FirstOrDefault();
        }

        public EditarTelaQueryResult Editar(int id)
        {
            var tela = Tela(id);
            if (tela == null)
                return null;

            return new EditarTelaQueryResult()
            {
                Id = tela.Id,
                IdAreaAtuacao = tela.AreaAtuacao.Id,
                AreaAtuacaoTitulo = tela.AreaAtuacao.Titulo,
                Titulo = tela.Titulo,
                Descricao = tela.Descricao,
                Link = tela.Link,
                DataCadastro = tela.DataCadastro,
                Ativo = tela.Ativo
            };
        }

        public BuscarTelaResumidoQueryResult BuscarPorId(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT        tela.Id, tela.Titulo, areaAtuacao.Titulo AS AreaAtuacao, tela.Ativo \n");
            query.Append("FROM 			TELAS tela \n");
            query.Append("INNER JOIN	AREAATUACAO areaAtuacao ON (tela.IdAreaAtuacao = areaAtuacao.Id) \n");
            query.Append("WHERE			tela.ID = :Id \n");
            query.Append("ORDER BY      tela.Id DESC");

            var tela = _context
                .Connection
                .Query<BuscarTelaResumidoQueryResult>(query.ToString(), new { Id = id });

            return tela.FirstOrDefault();
        }

        public IEnumerable<BuscarTelaResumidoQueryResult> BuscarPorNome(string nome)
        {
            var query = new StringBuilder();
            query.Append("SELECT        tela.Id, tela.Titulo, areaAtuacao.Titulo AS AreaAtuacao, tela.Ativo \n");
            query.Append("FROM 			TELAS tela \n");
            query.Append("INNER JOIN	AREAATUACAO areaAtuacao ON (tela.IdAreaAtuacao = areaAtuacao.Id) \n");
            query.Append("WHERE			tela.Titulo LIKE '%" + nome + "%' \n");
            query.Append("ORDER BY      tela.Id DESC");

            var telas = _context
                .Connection
                .Query<BuscarTelaResumidoQueryResult>(query.ToString(), new { });

            return telas;
        }

        public IEnumerable<BuscarTelaResumidoQueryResult> BuscarTodos()
        {
            var query = new StringBuilder();
            query.Append("SELECT        tela.Id, tela.Titulo, areaAtuacao.Titulo AS AreaAtuacao, tela.Ativo \n");
            query.Append("FROM 			TELAS tela \n");
            query.Append("INNER JOIN	AREAATUACAO areaAtuacao ON (tela.IdAreaAtuacao = areaAtuacao.Id) \n");
            query.Append("ORDER BY      tela.Id DESC");

            var telas = _context
                .Connection
                .Query<BuscarTelaResumidoQueryResult>(query.ToString(), new { });

            return telas;
        }

        public void Salvar(Tela tela)
        {
            var query = @"INSERT INTO Telas (IdAreaAtuacao, Titulo, Descricao, Link, DataCadastro, Ativo) VALUES (:IdAreaAtuacao, :Titulo, :Descricao, :Link, :DataCadastro, :Ativo) returning Id into :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add(name: "IdAreaAtuacao", value: tela.AreaAtuacao.Id, direction: ParameterDirection.Input);
            param.Add(name: "Titulo", value: tela.Titulo, direction: ParameterDirection.Input);
            param.Add(name: "Descricao", value: tela.Descricao, direction: ParameterDirection.Input);
            param.Add(name: "Link", value: tela.Link, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: tela.DataCadastro, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)tela.Ativo, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public void Atualizar(Tela tela)
        {
            var query = @"UPDATE Telas SET IdAreaAtuacao = :IdAreaAtuacao, Titulo = :Titulo, Descricao = :Descricao, Link = :Link, DataCadastro = :DataCadastro, Ativo = :Ativo WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: tela.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdAreaAtuacao", value: tela.AreaAtuacao.Id, direction: ParameterDirection.Input);
            param.Add(name: "Titulo", value: tela.Titulo, direction: ParameterDirection.Input);
            param.Add(name: "Descricao", value: tela.Descricao, direction: ParameterDirection.Input);
            param.Add(name: "Link", value: tela.Link, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: tela.DataCadastro, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)tela.Ativo, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        public void Excluir(int id)
        {
            var query = @"DELETE FROM Telas WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        public bool TelaExistente(int id)
        {
            var query = @"SELECT tela.* FROM Telas tela WHERE tela.Id = :Id";

            var result = _context.Connection.Query<bool>(query, new { Id = id });

            return result.Any();
        }

        public bool TelaExistente(string nome)
        {
            var query = @"SELECT tela.* FROM Telas tela WHERE tela.Titulo = :Nome";

            var result = _context.Connection.Query<bool>(query, new { Nome = nome });

            return result.Any();
        }
    }
}
