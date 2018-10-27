using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Queries;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Domain.UsuarioBaseContext.ValueObjects;
using V8Net.Infra.Data.DataContext;

namespace V8Net.Infra.Data.UsuarioBase.Repositories
{
    public class UsuarioBaseRepository : IUsuarioBaseRepository
    {
        private readonly V8NetDataContext _context;

        public UsuarioBaseRepository(V8NetDataContext context)
        {
            _context = context;
        }

        #region --> MÉTODOS: Usuário (principal)

        public BuscarUsuariosBaseResumidoQueryResult BuscarPorId(int id)
        {
            var query = @"SELECT Id, Usuario, Email, Documento, Ativo FROM UsuarioBase WHERE Id = :Id";

            var usuarioBase = _context
                                .Connection
                                .Query<BuscarUsuariosBaseResumidoQueryResult>(query, new { Id = id });

            return usuarioBase.FirstOrDefault();
        }

        public EditarUsuariosBaseQueryResult Editar(int id)
        {
            var usuario = UsuarioBase(id);
            if (usuario == null)
                return null;

            return new EditarUsuariosBaseQueryResult()
            {
                Id = usuario.Id,
                Usuario = usuario.Login.Usuario,
                Senha = usuario.Login.Senha,
                Email = usuario.Email.Email,
                Documento = usuario.Documento.Documento,
                ChaveDeAcesso = usuario.ChaveDeAcesso,
                DataCadastro = usuario.DataCadastro,
                Ativo = usuario.Ativo,
                PerfilAcesso = usuario.PerfilAcesso,
                ImpressoraZebra = usuario.ImpressoraZebra
            };
        }

        public IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarPorUsuario(string usuarionome)
        {
            var query = @"SELECT Id, Usuario, Email, Documento, Ativo FROM UsuarioBase WHERE Usuario = :Usuario ORDER BY Id DESC";

            var usuarioBase = _context
                                .Connection
                                .Query<BuscarUsuariosBaseResumidoQueryResult>(query, new { Usuario = usuarionome });

            return usuarioBase;
        }

        public IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarTodos()
        {
            var query = @"SELECT Id, Usuario, Email, Documento, Ativo Ativo FROM UsuarioBase ORDER BY Id DESC";

            var usuarios = _context
                            .Connection
                            .Query<BuscarUsuariosBaseResumidoQueryResult>(query, new { });

            #region TRICK --> Query para retornar um ID do tipo RAW para VARCHAR2
            //var query =
            //    string.Format("SELECT UTL_RAW.CAST_TO_VARCHAR2(Id) As Id, Usuario, Email, Documento, DataCadastro, Ativo FROM UsuarioBase");

            //return
            //    _context
            //    .Connection
            //    .Query<BuscarUsuariosBaseQueryResult>(query)
            //    .Select(x => new BuscarUsuariosBaseQueryResult { Id = new Guid(x.Id).ToString() });
            #endregion

            return usuarios;
        }

        public Domain.UsuarioBaseContext.Entities.UsuarioBase UsuarioBase(int id)
        {
            var query = @"SELECT Id, ChaveDeAcesso, DataCadastro, Ativo, PerfilAcesso, ImpressoraZebra, Usuario, Senha, Email, Documento FROM UsuarioBase WHERE Id = :Id";

            var usuarioBase = _context.Connection.Query<Domain.UsuarioBaseContext.Entities.UsuarioBase, LoginVO, EmailVO, DocumentoVO, Domain.UsuarioBaseContext.Entities.UsuarioBase>(query,
                    (usuario, loginVO, emailVO, documentoVO) =>
                    {
                        usuario.AtribuirUsuario(loginVO, emailVO, documentoVO);
                        return usuario;
                    },
                    new {Id = id},
                    splitOn: "Id, Usuario, Email, Documento")
                    .FirstOrDefault();

            if (usuarioBase != null)
                UsuarioBaseAtribuirRelacionamentos(usuarioBase);

            return usuarioBase;
        }

        public Domain.UsuarioBaseContext.Entities.UsuarioBase UsuarioBase(string email)
        {
            var query = @"SELECT Id, ChaveDeAcesso, DataCadastro, Ativo, PerfilAcesso, ImpressoraZebra, Usuario, Senha, Email, Documento FROM UsuarioBase WHERE Email = :Email";

            var usuarioBase = _context.Connection.Query<Domain.UsuarioBaseContext.Entities.UsuarioBase, LoginVO, EmailVO, DocumentoVO, Domain.UsuarioBaseContext.Entities.UsuarioBase>(query,
                    (usuario, loginVO, emailVO, documentoVO) =>
                    {
                        usuario.AtribuirUsuario(loginVO, emailVO, documentoVO);
                        return usuario;
                    },
                    new { Email = email },
                    splitOn: "Id, Usuario, Email, Documento")
                .FirstOrDefault();

            if (usuarioBase != null)
                UsuarioBaseAtribuirRelacionamentos(usuarioBase);

            return usuarioBase;
        }

        public Domain.UsuarioBaseContext.Entities.UsuarioBase UsuarioBase(string usuariologin, string senha)
        {
            var query = @"SELECT Id, ChaveDeAcesso, DataCadastro, Ativo, PerfilAcesso, ImpressoraZebra, Usuario, Senha, Email, Documento FROM UsuarioBase WHERE Usuario = :Usuario AND Senha = :Senha";

            var usuarioBase = _context.Connection.Query<Domain.UsuarioBaseContext.Entities.UsuarioBase, LoginVO, EmailVO, DocumentoVO, Domain.UsuarioBaseContext.Entities.UsuarioBase>(query,
                (usuario, loginVO, emailVO, documentoVO) =>
                {
                    usuario.AtribuirUsuario(loginVO, emailVO, documentoVO);
                    return usuario;
                },
                new { Usuario = usuariologin, Senha = senha },
                splitOn: "Id, Usuario, Email, Documento")
                .FirstOrDefault();

            if (usuarioBase != null)
                UsuarioBaseAtribuirRelacionamentos(usuarioBase);

            return usuarioBase;
        }

        public void Salvar(Domain.UsuarioBaseContext.Entities.UsuarioBase usuario)
        {
            var query = new StringBuilder();
            query.Append("INSERT INTO UsuarioBase \n");
            query.Append("(Usuario, Senha, Email, Documento, ChaveDeAcesso, DataCadastro, Ativo, PerfilAcesso, ImpressoraZebra) \n");
            query.Append("VALUES \n");
            query.Append("(:Usuario, :Senha, :Email, :Documento, :ChaveDeAcesso, :DataCadastro, :Ativo, :PerfilAcesso, :ImpressoraZebra) returning Id into :Id");

            var param = new DynamicParameters();
            param.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add(name: "Usuario", value: usuario.Login.Usuario, direction: ParameterDirection.Input);
            param.Add(name: "Senha", value: usuario.Login.Senha, direction: ParameterDirection.Input);
            param.Add(name: "Email", value: usuario.Email.Email, direction: ParameterDirection.Input);
            param.Add(name: "Documento", value: usuario.Documento.Documento, direction: ParameterDirection.Input);
            param.Add(name: "ChaveDeAcesso", value: usuario.ChaveDeAcesso, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: usuario.DataCadastro, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)usuario.Ativo, direction: ParameterDirection.Input);
            param.Add(name: "PerfilAcesso", value: (int)usuario.PerfilAcesso, direction: ParameterDirection.Input);
            param.Add(name: "ImpressoraZebra", value: usuario.ImpressoraZebra, direction: ParameterDirection.Input);

            _context.Connection.Execute(query.ToString(), param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }        

        public void Atualizar(Domain.UsuarioBaseContext.Entities.UsuarioBase usuario)
        {
            var query = new StringBuilder();
            query.Append("UPDATE UsuarioBase SET \n");
            query.Append("Usuario = :Usuario, Senha = :Senha, Email = :Email, Documento = :Documento, Ativo = :Ativo, PerfilAcesso = :PerfilAcesso, ImpressoraZebra = :ImpressoraZebra \n");
            query.Append("WHERE Id = :Id");

            var param = new DynamicParameters();
            param.Add(name: "Id", value: usuario.Id, direction: ParameterDirection.Input);
            param.Add(name: "Usuario", value: usuario.Login.Usuario, direction: ParameterDirection.Input);
            param.Add(name: "Senha", value: usuario.Login.Senha, direction: ParameterDirection.Input);
            param.Add(name: "Email", value: usuario.Email.Email, direction: ParameterDirection.Input);
            param.Add(name: "Documento", value: usuario.Documento.Documento, direction: ParameterDirection.Input);
            param.Add(name: "Ativo", value: (int)usuario.Ativo, direction: ParameterDirection.Input);
            param.Add(name: "PerfilAcesso", value: (int)usuario.PerfilAcesso, direction: ParameterDirection.Input);
            param.Add(name: "ImpressoraZebra", value: usuario.ImpressoraZebra, direction: ParameterDirection.Input);

            _context.Connection.Execute(query.ToString(), param);
        }

        public void Excluir(int id)
        {
            var queryUsuarioEmpresa = @"DELETE FROM UsuariosEmpresas WHERE IdUsuario = :Id";
            var queryUsuarioAtuacao = @"DELETE FROM UsuariosAreasAtuacao WHERE IdUsuario = :Id";
            var queryUsuarioTela = @"DELETE FROM UsuarioTelas WHERE IdUsuario = :Id";
            var queryUsuario = @"DELETE FROM UsuarioBase WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(queryUsuarioEmpresa, param);
            _context.Connection.Execute(queryUsuarioAtuacao, param);
            _context.Connection.Execute(queryUsuarioTela, param);
            _context.Connection.Execute(queryUsuario, param);
        }

        public bool DocumentoExistente(string documento)
        {
            var query = @"SELECT usuario.* FROM UsuarioBase usuario WHERE usuario.Documento = :Documento";

            var result = _context
                            .Connection
                            .Query<bool>(query, new { Documento  = documento });

            return result.Any();
        }

        public bool UsuarioExistente(string usuario)
        {
            var query = @"SELECT usuario.* FROM UsuarioBase usuario WHERE usuario.Usuario = :Usuario";

            var result = _context
                .Connection
                .Query<bool>(query, new { Usuario = usuario });

            return result.Any();
        }

        public bool EmailExistente(string email)
        {
            var query = @"SELECT usuario.* FROM UsuarioBase usuario WHERE usuario.Email = :Email";

            var result = _context
                .Connection
                .Query<bool>(query, new { Email = email });

            return result.Any();
        }

        public Domain.UsuarioBaseContext.Entities.UsuarioBase UsuarioBaseAtribuirRelacionamentos(Domain.UsuarioBaseContext.Entities.UsuarioBase usuarioBase)
        {
            usuarioBase.AtribuirUsuarioEmpresas(UsuarioEmpresas(usuarioBase.Id));
            usuarioBase.AtribuirUsuarioAtuacoes(UsuarioAtuacoes(usuarioBase.Id));
            usuarioBase.AtribuirUsuarioTelas(UsuarioTelas(usuarioBase.Id));

            return usuarioBase;
        }

        #endregion

        #region --> MÉTODOS: Usuário Login

        public void ConfigurarSenhaTemporaria(int idUsuario, string senha)
        {
            // Atualiza uma nova senha p/ o usuário
            var query01 = @"UPDATE UsuarioBase SET Senha = :Senha WHERE Id = :Id";
            // Desativa todas as senhas temporários que já foram criadas p/ o usuário
            var query02 = @"UPDATE UsuarioSenhaTemporaria SET Ativo = 0 WHERE IdUsuario = :IdUsuario";
            // Insere e ativa uma nova senha temporária /p o usuário
            var query03 = @"INSERT INTO UsuarioSenhaTemporaria (IdUsuario, SenhaTemporaria, DataCadastro, Ativo) VALUES (:IdUsuario, :SenhaTemporaria, :DataCadastro, 1)";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: idUsuario, direction: ParameterDirection.Input);
            param.Add(name: "Senha", value: senha, direction: ParameterDirection.Input);
            param.Add(name: "IdUsuario", value: idUsuario, direction: ParameterDirection.Input);
            param.Add(name: "SenhaTemporaria", value: senha, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: DateTime.Now, direction: ParameterDirection.Input);

            _context.Connection.Execute(query01, param);
            _context.Connection.Execute(query02, param);
            _context.Connection.Execute(query03, param);
        }

        public BuscarUsuarioSenhaTemporariaQueryResult BuscarUsuarioSenhaTemporaria(int id)
        {
            var query = @"SELECT senhaTemporaria.* FROM UsuarioSenhaTemporaria senhaTemporaria WHERE IdUsuario = :Id";

            var result = _context.Connection.Query<BuscarUsuarioSenhaTemporariaQueryResult>(query, new { Id = id });

            return result
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        #endregion

        #region --> MÉTODOS: Usuário Empresa

        public Empresa Empresa(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT    Codigo_Empresa AS Id, Nome_Empresa AS Nome, Nome_Fantasia AS Fantasia, Telefone AS Telefone, \n");
            query.Append("          Tipo_Empresa AS TipoEmpresa, Cgc_9 AS Cgc9, Cgc_4 AS Cgc4, Cgc_2 AS Cgc2 \n");
            query.Append("FROM      Empresas \n");
            query.Append("WHERE     Codigo_Empresa = :Id");

            var empresa = _context.Connection.Query<Empresa>(query.ToString(), new { Id = id });

            return empresa.FirstOrDefault();
        }

        public IEnumerable<Empresa> UsuarioEmpresas(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT        empresa.Codigo_Empresa AS Id, empresa.Nome_Empresa AS Nome, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Tipo_Empresa AS TipoEmpresa, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM          UsuarioBase usuario \n");
            query.Append("INNER JOIN 	UsuariosEmpresas usuarioEmpresa ON (usuario.Id = usuarioEmpresa.IdUsuario) \n");
            query.Append("INNER JOIN	Empresas empresa ON (usuarioEmpresa.IdEmpresa = empresa.Codigo_Empresa) \n");
            query.Append("WHERE 		usuario.Id = :idUsuario \n");
            query.Append("ORDER BY 		usuarioEmpresa.IdEmpresa ASC");

            var empresas = _context.Connection.Query<Empresa>(query.ToString(), new { idUsuario = id });

            return empresas;
        }

        public IEnumerable<BuscarUsuarioEmpresasResumidoQueryResult> BuscarEmpresasPorUsuario(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT        empresa.Codigo_Empresa AS Id, empresa.Nome_Fantasia AS Fantasia, empresa.Telefone AS Telefone, empresa.Tipo_Empresa AS TipoEmpresa, empresa.Cgc_9 AS Cgc9, empresa.Cgc_4 AS Cgc4, empresa.Cgc_2 AS Cgc2 \n");
            query.Append("FROM          UsuarioBase usuario \n");
            query.Append("INNER JOIN 	UsuariosEmpresas usuarioEmpresa ON (usuario.Id = usuarioEmpresa.IdUsuario) \n");
            query.Append("INNER JOIN	Empresas empresa ON (usuarioEmpresa.IdEmpresa = empresa.Codigo_Empresa) \n");
            query.Append("WHERE 		usuario.Id = :idUsuario \n");
            query.Append("ORDER BY 		usuarioEmpresa.IdEmpresa ASC");

            var empresas = _context.Connection.Query<BuscarUsuarioEmpresasResumidoQueryResult>(query.ToString(), new { idUsuario = id });

            return empresas;
        }

        public void SalvarUsuarioEmpresa(UsuarioEmpresa usuarioEmpresa)
        {
            var query = @"INSERT INTO UsuariosEmpresas (IdUsuario, IdEmpresa) VALUES (:IdUsuario, :IdEmpresa) returning Id into :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: usuarioEmpresa.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdUsuario", value: usuarioEmpresa.IdUsuario, direction: ParameterDirection.Input);
            param.Add(name: "IdEmpresa", value: usuarioEmpresa.IdEmpresa, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public bool UsuarioEmpresaExistente(int idUsuario, int idEmpresa)
        {
            var query = new StringBuilder();
            query.Append("SELECT        usuarioEmpresa.Id, usuarioEmpresa.IdUsuario, usuarioEmpresa.IdEmpresa \n");
            query.Append("FROM          UsuarioBase usuario \n");
            query.Append("INNER JOIN 	UsuariosEmpresas usuarioEmpresa ON (usuario.Id = usuarioEmpresa.IdUsuario) \n");
            query.Append("INNER JOIN	Empresas empresa ON (usuarioEmpresa.IdEmpresa = empresa.Codigo_Empresa) \n");
            query.Append("WHERE 		usuario.Id = :IdUsuario AND \n");
            query.Append("              empresa.Codigo_Empresa = :IdEmpresa");

            var result = _context.Connection.Query<bool>(query.ToString(), new
            {
                IdUsuario = idUsuario,
                IdEmpresa = idEmpresa
            });

            return result.Any();
        }

        public bool UsuarioEmpresaExistente(int id)
        {
            var query = @"SELECT usuarioEmpresa.* FROM UsuariosEmpresas usuarioEmpresa WHERE usuarioEmpresa.Id = :Id";
            
            var usuarioEmpresa = _context.Connection.Query<bool>(query, new { Id = id });

            return usuarioEmpresa.FirstOrDefault();
        }

        public void ExcluirUsuarioEmpresa(int id)
        {
            var query = @"DELETE FROM UsuariosEmpresas WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        #endregion

        #region --> MÉTODOS: Usuário Atuação

        public AreaAtuacao AreaAtuacao(int id)
        {
            var query = @"SELECT Id, Titulo, Descricao, DataCadastro, Ativo FROM AreaAtuacao WHERE Id = :Id";

            var areasAtuacao = _context.Connection.Query<AreaAtuacao>(query.ToString(), new { Id = id });

            return areasAtuacao.FirstOrDefault();
        }

        public IEnumerable<AreaAtuacao> UsuarioAtuacoes(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT 		areaatuacao.Id, areaatuacao.Titulo, areaatuacao.Descricao, areaatuacao.DataCadastro, areaatuacao.Ativo \n");
            query.Append("FROM 			UsuarioBase usuario \n");
            query.Append("INNER JOIN    UsuariosAreasAtuacao usuarioatuacao ON (usuario.Id = usuarioatuacao.IdUsuario) \n");
            query.Append("INNER JOIN    AreaAtuacao areaatuacao ON (usuarioatuacao.IdAreaAtuacao = areaatuacao.Id) \n");
            query.Append("WHERE 		usuario.Id = :Id \n");
            query.Append("ORDER BY 		areaatuacao.Id ASC");

            var areasAtuacao = _context.Connection.Query<AreaAtuacao>(query.ToString(), new { Id = id });

            return areasAtuacao;
        }

        public IEnumerable<BuscarUsuarioAtuacoesQueryResult> BuscarAtuacoesPorUsuario(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT 		areaatuacao.Id, areaatuacao.Titulo, areaatuacao.Descricao \n");
            query.Append("FROM 			UsuarioBase usuario \n");
            query.Append("INNER JOIN    UsuariosAreasAtuacao usuarioatuacao ON (usuario.Id = usuarioatuacao.IdUsuario) \n");
            query.Append("INNER JOIN    AreaAtuacao areaatuacao ON (usuarioatuacao.IdAreaAtuacao = areaatuacao.Id) \n");
            query.Append("WHERE 		usuario.Id = :Id \n");
            query.Append("ORDER BY 		areaatuacao.Id ASC");

            var areasAtuacao = _context.Connection.Query<BuscarUsuarioAtuacoesQueryResult>(query.ToString(), new { Id = id });

            return areasAtuacao;
        }

        public void SalvarUsuarioAtuacao(UsuarioAreaAtuacao areaAtuacao)
        {
            var query = "INSERT INTO UsuariosAreasAtuacao (IdUsuario, IdAreaAtuacao) VALUES (:IdUsuario, :IdAreaAtuacao) returning Id into :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: areaAtuacao.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdUsuario", value: areaAtuacao.Usuario.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdAreaAtuacao", value: areaAtuacao.AreaAtuacao.Id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public bool UsuarioAtuacaoExistente(int idUsuario, int idAreaAtuacao)
        {
            var query = new StringBuilder();
            query.Append("SELECT	  	usuarioAtuacao.* \n");
            query.Append("FROM			UsuarioBase usuario \n");
            query.Append("INNER JOIN	UsuariosAreasAtuacao usuarioAtuacao ON (usuario.Id = usuarioAtuacao.IdUsuario) \n");
            query.Append("INNER JOIN	AreaAtuacao atuacao ON (usuarioAtuacao.IdAreaAtuacao = atuacao.Id) \n");
            query.Append("WHERE			usuario.Id = :IdUsuario AND \n");
            query.Append("              atuacao.Id = :IdAreaAtuacao");

            var result = _context.Connection.Query<bool>(query.ToString(), new
            {
                IdUsuario = idUsuario,
                IdAreaAtuacao = idAreaAtuacao
            });

            return result.Any();
        }

        public bool UsuarioAtuacaoExistente(int id)
        {
            var query = "SELECT usuarioAtuacao.* FROM UsuariosAreasAtuacao usuarioAtuacao WHERE usuarioAtuacao.Id = :Id";

            var result = _context.Connection.Query<bool>(query, new { Id = id });

            return result.Any();
        }

        public void ExcluirUsuarioAtuacao(int id)
        {
            var query = "DELETE FROM UsuariosAreasAtuacao WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        #endregion

        #region --> MÉTODOS: Usuário Tela

        public Tela Tela(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT	    tela.Id, tela.idAreaAtuacao, tela.Titulo, tela.Descricao, tela.Link, tela.DataCadastro, tela.Ativo, \n");
            query.Append("              areaatuacao.Id, areaatuacao.Titulo, areaatuacao.Descricao, areaatuacao.DataCadastro, areaatuacao.Ativo \n");
            query.Append("FROM			Telas tela \n");
            query.Append("INNER JOIN    AreaAtuacao areaatuacao ON (tela.IdAreaAtuacao = areaatuacao.Id)  \n");
            query.Append("WHERE			tela.Id = :Id");

            var tela = _context.Connection.Query<Tela, AreaAtuacao, Tela>(query.ToString(),
                (t, aa) =>
                {
                    t.AtribuirAreaAtuacao(aa);
                    return t;
                },
                new { Id = id });

            return tela.FirstOrDefault();
        }

        public EditarUsuarioTelaQueryResult EditarUsuarioTela(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT	  	permissao.Id, permissao.IdUsuario, permissao.IdTela, permissao.Incluir, permissao.Atualizar, permissao.Excluir, permissao.Consultar, permissao.DataCadastro \n");
            query.Append("FROM			UsuarioTelas permissao \n");
            query.Append("WHERE			permissao.Id = :Id \n");

            var usuarioTela = _context.Connection.Query<EditarUsuarioTelaQueryResult>(query.ToString(),
                new { Id = id }).FirstOrDefault();
            
            if (usuarioTela == null) return null;

            return new EditarUsuarioTelaQueryResult()
            {
                Id = usuarioTela.Id,
                IdUsuario = usuarioTela.IdUsuario,
                IdTela = usuarioTela.IdTela,
                Incluir = usuarioTela.Incluir,
                Atualizar = usuarioTela.Atualizar,
                Excluir = usuarioTela.Excluir,
                Consultar = usuarioTela.Consultar,
                DataCadatro = usuarioTela.DataCadatro
            };
        }

        public UsuarioTela UsuarioTela(int id, Domain.UsuarioBaseContext.Entities.UsuarioBase usuario, Tela tela)
        {
            var query = new StringBuilder();
            query.Append("SELECT	  	permissao.Id, permissao.Incluir, permissao.Atualizar, permissao.Excluir, permissao.Consultar, permissao.DataCadastro \n");
            query.Append("FROM			UsuarioTelas permissao \n");
            query.Append("INNER JOIN	UsuarioBase usuario ON (permissao.IdUsuario = usuario.Id) \n");
            query.Append("INNER JOIN	Telas tela ON (permissao.IdTela = tela.Id) \n");
            query.Append("WHERE			permissao.Id = :Id AND usuario.Id = :IdUsuario AND tela.Id = :IdTela \n");

            var usuarioTela = _context.Connection.Query<UsuarioTela>(query.ToString(), 
                    new{ Id = id, IdUsuario = usuario.Id, IdTela = tela.Id }).FirstOrDefault();

            if (usuarioTela == null) return null;

            usuarioTela.AtribuirUsuario(usuario);
            usuarioTela.AtribuirTela(tela);

            return usuarioTela;
        }

        public IEnumerable<Tela> UsuarioTelas(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT	    tela.Id, tela.idAreaAtuacao, tela.Titulo, tela.Descricao, tela.Link, tela.DataCadastro, tela.Ativo, \n");
            query.Append("              areaatuacao.Id, areaatuacao.Titulo, areaatuacao.Descricao, areaatuacao.DataCadastro, areaatuacao.Ativo \n");
            query.Append("FROM			UsuarioTelas permissao \n");
            query.Append("INNER JOIN	UsuarioBase usuario ON (permissao.IdUsuario = usuario.Id) \n");
            query.Append("INNER JOIN	Telas tela ON (permissao.IdTela = tela.Id) \n");
            query.Append("INNER JOIN    AreaAtuacao areaatuacao ON (tela.IdAreaAtuacao = areaatuacao.Id) \n");
            query.Append("WHERE			usuario.Id = :Id \n");
            query.Append("ORDER BY		tela.Id ASC");

            var telas = _context.Connection.Query<Tela, AreaAtuacao, Tela>(query.ToString(),
                (t, aa) => 
                {
                    t.AtribuirAreaAtuacao(aa);
                    return t;
                },
                new { Id = id });

            return telas;
        }

        public IEnumerable<BuscarUsuarioTelasQueryResult> BuscarTelasPorUsuario(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT	    tela.Id, tela.Titulo, tela.Descricao, tela.Link \n");
            query.Append("FROM			UsuarioTelas permissao \n");
            query.Append("INNER JOIN	UsuarioBase usuario ON (permissao.IdUsuario = usuario.Id) \n");
            query.Append("INNER JOIN	Telas tela ON (permissao.IdTela = tela.Id) \n");
            query.Append("INNER JOIN    AreaAtuacao areaatuacao ON (tela.IdAreaAtuacao = areaatuacao.Id) \n");
            query.Append("WHERE			usuario.Id = :Id \n");
            query.Append("ORDER BY		tela.Id ASC");

            var telas = _context.Connection.Query<BuscarUsuarioTelasQueryResult>(query.ToString(), new { Id = id });

            return telas;
        }

        public void SalvarUsuarioTela(UsuarioTela usuarioTela)
        {
            var query = @"INSERT INTO UsuarioTelas (IdUsuario, IdTela, Incluir, Atualizar, Excluir, Consultar, DataCadastro) VALUES (:IdUsuario, :IdTela, :Incluir, :Atualizar, :Excluir, :Consultar, :DataCadastro) returning Id into :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add(name: "IdUsuario", value: usuarioTela.UsuarioBase.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdTela", value: usuarioTela.Tela.Id, direction: ParameterDirection.Input);
            param.Add(name: "Incluir", value: (int)usuarioTela.Incluir, direction: ParameterDirection.Input);
            param.Add(name: "Atualizar", value: (int)usuarioTela.Atualizar, direction: ParameterDirection.Input);
            param.Add(name: "Excluir", value: (int)usuarioTela.Excluir, direction: ParameterDirection.Input);
            param.Add(name: "Consultar", value: (int)usuarioTela.Consultar, direction: ParameterDirection.Input);
            param.Add(name: "DataCadastro", value: usuarioTela.DataCadatro, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);

            // Retorno do id gerado na base
            //var Id = param.Get<int>("Id");
        }

        public void AtualizarUsuarioTela(UsuarioTela usuarioTela)
        {
            var query = "UPDATE UsuarioTelas SET Incluir = :Incluir, Atualizar = :Atualizar, Excluir = :Excluir, Consultar = :Consultar WHERE Id = :Id AND IdUsuario = :IdUsuario AND IdTela = :IdTela";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: usuarioTela.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdUsuario", value: usuarioTela.UsuarioBase.Id, direction: ParameterDirection.Input);
            param.Add(name: "IdTela", value: usuarioTela.Tela.Id, direction: ParameterDirection.Input);
            param.Add(name: "Incluir", value: (int)usuarioTela.Incluir, direction: ParameterDirection.Input);
            param.Add(name: "Atualizar", value: (int)usuarioTela.Atualizar, direction: ParameterDirection.Input);
            param.Add(name: "Excluir", value: (int)usuarioTela.Excluir, direction: ParameterDirection.Input);
            param.Add(name: "Consultar", value: (int)usuarioTela.Consultar, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        public bool UsuarioTelaExistente(int idUsuario, int idTela)
        {
            var query = new StringBuilder();
            query.Append("SELECT	    permissao.* \n");
            query.Append("FROM			UsuarioTelas permissao \n");
            query.Append("INNER JOIN	UsuarioBase usuario ON (permissao.IdUsuario = usuario.Id)  \n");
            query.Append("INNER JOIN	Telas tela ON (permissao.IdTela = tela.Id)  \n");
            query.Append("WHERE			usuario.Id = :IdUsuario AND \n");
            query.Append("              tela.Id = :IdTela");

            var result = _context.Connection.Query<bool>(query.ToString(), new
            {
                IdUsuario = idUsuario,
                IdTela = idTela
            });

            return result.Any();
        }

        public bool UsuarioTelaExistente(int id)
        {
            var query = @"SELECT tela.* FROM UsuarioTelas tela WHERE tela.Id = :Id";

            var result = _context.Connection.Query<bool>(query, new { Id = id });

            return result.Any();
        }

        public void ExcluirUsuarioTela(int id)
        {
            var query = @"DELETE FROM UsuarioTelas WHERE Id = :Id";

            var param = new DynamicParameters();
            param.Add(name: "Id", value: id, direction: ParameterDirection.Input);

            _context.Connection.Execute(query, param);
        }

        #endregion
    }
}