using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using V8Net.Domain.UsuarioBaseContext.Commands.Handlers;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Queries;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Api.Controllers.UsuarioBaseContext
{
    public class UsuarioBaseController : Controller
    {
        private readonly IUsuarioBaseRepository _repository;
        private readonly UsuarioBaseHandler _handler;

        public UsuarioBaseController(IUsuarioBaseRepository repository, UsuarioBaseHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        #region --> Usuário Base (principal)

        /// <summary>
        /// Retorna uma lista resumida contendo todos os usuários cadastrados na base de dados.
        /// </summary>
        /// <returns>Objeto contendo os dados resumidos do usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/usuarios")]
        public IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarTodos()
        {
            return _repository.BuscarTodos();
        }

        /// <summary>
        /// Retorna um usuário pelo id.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Objeto contendo um resumo dos dados de usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/usuario-por-id/{id}")]
        public BuscarUsuariosBaseResumidoQueryResult BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }

        /// <summary>
        /// Retorna uma lista resumida de usuários pelo nome.
        /// </summary>
        /// <param name="nome">Nome do usuário</param>
        /// <returns>Objeto contendo um resumo dos dados de usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/usuario-por-nome/{nome}")]
        public IEnumerable<BuscarUsuariosBaseResumidoQueryResult> BuscarPorUsuario(string nome)
        {
            return _repository.BuscarPorUsuario(nome);
        }

        /// <summary>
        /// Cria um novo usuário na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/novo-usuario")]
        public ICommandResult Criar([FromBody]CriarUsuarioBaseCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Retorna os dados para edição do usuário pelo id.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Objeto contendo os dados completos do usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/editar-usuario/{id}")]
        public EditarUsuariosBaseQueryResult Editar(int id)
        {
            return _repository.Editar(id);
        }

        /// <summary>
        /// Atualiza um usuário na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPut]
        [Route("v8net/v1/atualizar-usuario")]
        public ICommandResult Atualizar([FromBody]AtualizarUsuarioBaseCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// exclui um usuário na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-usuario")]
        public ICommandResult Excluir([FromBody]ExcluirUsuarioBaseCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Cria uma nova senha temporária para o usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("v8net/v1/usuario-recuperar-senha")]
        public ICommandResult RecuperarSenha([FromBody]RecuperarSenhaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        #endregion

        #region --> Usuário Empresa    

        /// <summary>
        /// Acrescenta uma nova empresa para o usuário.
        /// </summary>
        /// <returns>Classe com as propriedades: Success(boolean), Message(string) e Data(usuario, documento).</returns>
        [HttpPost]
        [Route("v8net/v1/novo-usuario-empresa")]
        public ICommandResult Criar([FromBody]CriarUsuarioEmpresaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui uma empresa relacionada ao usuário.
        /// </summary>
        /// <returns>Classe com as propriedades: Success(boolean), Message(string) e Data(usuario, documento).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-usuario-empresa")]
        public ICommandResult ExcluirUsuarioEmpresa([FromBody]ExcluirUsuarioEmpresaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Retorna uma lista resumida de empresas relacionadas ao id do usuário.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Objeto contendo os dados resumidos das empresas relacionadas ao usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/usuario-empresas/{id}")]
        public IEnumerable<BuscarUsuarioEmpresasResumidoQueryResult> BuscarEmpresasPorUsuario(int id)
        {
            return _repository.BuscarEmpresasPorUsuario(id);
        }

        #endregion

        #region --> Usuário Área de Atuação

        /// <summary>
        /// Acrescenta uma nova área de atuação ao usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/novo-usuario-atuacao")]
        public ICommandResult Criar([FromBody]CriarUsuarioAtuacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui uma área de atuação relacionada ao usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-usuario-atuacao")]
        public ICommandResult ExcluirUsuarioAtuacao([FromBody]ExcluirUsuarioAtuacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Retorna uma lista resumida das áreas de atuação relacionadas ao id usuário.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Classes: Success(boolean), Message(string) e Data(object).</returns>
        [HttpGet]
        [Route("v8net/v1/usuario-atuacoes/{id}")]
        public IEnumerable<BuscarUsuarioAtuacoesQueryResult> BuscarAtuacoesPorUsuario(int id)
        {
            return _repository.BuscarAtuacoesPorUsuario(id);
        }

        #endregion

        #region --> Usuário Tela

        /// <summary>
        /// Acrescenta uma nova tela e permissões ao usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/novo-usuario-tela")]
        public ICommandResult Criar([FromBody]CriarUsuarioTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Retorna uma lista resumida contendo as telas e permissões relacionadas ao id do usuário.
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Objeto contendo os dados resumidos das telas relacionadas ao usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/usuario-telas/{id}")]
        public IEnumerable<BuscarUsuarioTelasQueryResult> BuscarTelasUsuario(int id)
        {
            return _repository.BuscarTelasPorUsuario(id);
        }

        /// <summary>
        /// Retorna a tela e permissões relacionadas ao id da permissão do usuário.
        /// </summary>
        /// <param name="id">Id da tela (permissão)</param>
        /// <returns>Objeto contendo os dados resumidos da tela e permissões relacionadas ao usuário.</returns>
        [HttpGet]
        [Route("v8net/v1/editar-usuario-tela/{id}")]
        public EditarUsuarioTelaQueryResult EditarUsuarioTela(int id)
        {
            return _repository.EditarUsuarioTela(id);
        }

        /// <summary>
        /// Atualiza as permissões de tela relacionadas ao usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPut]
        [Route("v8net/v1/atualizar-usuario-tela")]
        public ICommandResult Atualizar([FromBody]AtualizarUsuarioTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui uma tela e as permissões relacionadas ao usuário.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-usuario-tela")]
        public ICommandResult ExcluirUsuarioTela([FromBody]ExcluirUsuarioTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        #endregion
    }
}
