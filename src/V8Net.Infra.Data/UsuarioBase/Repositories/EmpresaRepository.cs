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
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly V8NetDataContext _context;

        public EmpresaRepository(V8NetDataContext context)
        {
            _context = context;
        }

        public Empresa Empresa(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT    empresa.Codigo_Empresa AS Id, empresa.Nome_Empresa AS Nome, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Tipo_Empresa AS TipoEmpresa, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas empresa \n");
            query.Append("WHERE 	empresa.Codigo_Empresa = :Id");

            var empresa = _context.Connection.Query<Empresa>(query.ToString(), new { Id = id });

            return empresa.FirstOrDefault();
        }

        public Empresa Empresa(string nome)
        {
            var query = new StringBuilder();
            query.Append("SELECT    empresa.Codigo_Empresa AS Id, empresa.Nome_Empresa AS Nome, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Tipo_Empresa AS TipoEmpresa, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas empresa \n");
            query.Append("WHERE 	empresa.Nome_Empresa = :Nome");

            var empresa = _context.Connection.Query<Empresa>(query.ToString(), new { Nome = nome });

            return empresa.FirstOrDefault();
        }

        public EditarEmpresaQueryResult Editar(int id)
        {
            var empresa = Empresa(id);
            if (empresa == null)
                return null;

            return new EditarEmpresaQueryResult()
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                Fantasia = empresa.Fantasia,
                Telefone = empresa.Telefone,
                TipoEmpresa = empresa.TipoEmpresa,
                Cgc9 = empresa.Cgc9,
                Cgc4 = empresa.Cgc4,
                Cgc2 = empresa.Cgc2
            };
        }

        public BuscarEmpresasResumidoQueryResult BuscarPorId(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT    empresa.Codigo_Empresa AS Id, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas empresa \n");
            query.Append("WHERE 	empresa.Codigo_Empresa = :Id");

            var empresas = _context.Connection.Query<BuscarEmpresasResumidoQueryResult>(query.ToString(), new { Id = id });

            return empresas.FirstOrDefault();
        }

        public IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarTodos()
        {
            var query = new StringBuilder();
            query.Append("SELECT    empresa.Codigo_Empresa AS Id, empresa.Nome_Empresa AS Nome, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas empresa \n");
            query.Append("ORDER BY 	empresa.Codigo_Empresa DESC");

            var empresas = _context.Connection.Query<BuscarEmpresasResumidoQueryResult>(query.ToString(), new { });

            return empresas;
        }

        public IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarPorNome(string nome)
        {
            var query = new StringBuilder();
            query.Append("SELECT    empresa.Codigo_Empresa AS Id, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas empresa \n");
            query.Append("WHERE 	empresa.Nome_Fantasia LIKE '%" + nome + "%'");

            var empresas = _context.Connection.Query<BuscarEmpresasResumidoQueryResult>(query.ToString(), new { });

            return empresas;
        }

        public void Salvar(Empresa empresa)
        {
            var query = @"INSERT INTO Empresas (Codigo_Empresa, Nome_Empresa, Nome_Fantasia, Telefone, Tipo_Empresa, Cgc_9, Cgc_4, Cgc_2) VALUES (:Id, :Nome, :Fantasia, :Telefone, :TipoEmpresa, :Cgc9, :Cgc4, :Cgc2) returning Id into :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: empresa.Id, direction: ParameterDirection.Input);
            param.Add(name: "Nome", value: empresa.Nome, direction: ParameterDirection.Input);
            param.Add(name: "Fantasia", value: empresa.Fantasia, direction: ParameterDirection.Input);
            param.Add(name: "Telefone", value: empresa.Telefone, direction: ParameterDirection.Input);
            param.Add(name: "TipoEmpresa", value: (int)empresa.TipoEmpresa, direction: ParameterDirection.Input);
            param.Add(name: "Cgc9", value: empresa.Cgc9, direction: ParameterDirection.Input);
            param.Add(name: "Cgc4", value: empresa.Cgc4, direction: ParameterDirection.Input);
            param.Add(name: "Cgc2", value: empresa.Cgc2, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public void Atualizar(Empresa empresa)
        {
            var query = @"UPDATE Empresas SET Nome_Empresa = :Nome, Nome_Fantasia = :Fantasia, Telefone = :Telefone, Tipo_Empresa = :TipoEmpresa, Cgc_9 = :Cgc9, Cgc_4 = :Cgc4, Cgc_2 = :Cgc2 WHERE Codigo_Empresa = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: empresa.Id, direction: ParameterDirection.Input);
            param.Add(name: "Nome", value: empresa.Nome, direction: ParameterDirection.Input);
            param.Add(name: "Fantasia", value: empresa.Fantasia, direction: ParameterDirection.Input);
            param.Add(name: "Telefone", value: empresa.Telefone, direction: ParameterDirection.Input);
            param.Add(name: "TipoEmpresa", value: (int)empresa.TipoEmpresa, direction: ParameterDirection.Input);
            param.Add(name: "Cgc9", value: empresa.Cgc9, direction: ParameterDirection.Input);
            param.Add(name: "Cgc4", value: empresa.Cgc4, direction: ParameterDirection.Input);
            param.Add(name: "Cgc2", value: empresa.Cgc2, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        public bool EmpresaExistente(int id)
        {
            var query = @"SELECT empresa.* FROM Empresas empresa WHERE empresa.Codigo_Empresa = :Id";

            var result = _context.Connection.Query<bool>(query, new  { Id = id });

            return result.Any();
        }

        public bool EmpresaExistente(string nome)
        {
            var query = @"SELECT empresa.* FROM Empresas empresa WHERE empresa.Nome_Empresa = :Nome";

            var result = _context.Connection.Query<bool>(query, new { Nome = nome });

            return result.Any();
        }

        public void Excluir(int id)
        {
            var query = @"DELETE FROM Empresas WHERE Codigo_Empresa = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }
    }
}
