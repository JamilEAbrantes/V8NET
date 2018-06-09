using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using V8Net.Domain.UsuarioBaseContext.Commands.Handlers;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Queries;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Api.Controllers.UsuarioBaseContext
{
    public class EmpresaController : Controller
    {
        private readonly IEmpresaRepository _repository;
        private readonly EmpresaHandler _handler;
        
        public EmpresaController(IEmpresaRepository repository, EmpresaHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        /// <summary>
        /// Retorna uma lista resumida das empresas cadastradas na base de dados.
        /// </summary>
        /// <returns>Objeto contendo os dados resumidos das empresa.</returns>
        [HttpGet]
        [Route("v8net/v1/empresas")]
        public IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarTodas()
        {
            return _repository.BuscarTodos();
        }

        /// <summary>
        /// Retorna uma lista resumida das empresas pelo nome.
        /// </summary>
        /// <param name="nome">Nome da empresa</param>
        /// <returns>Objeto contendo os dados resumidos das empresa.</returns>
        [HttpGet]
        [Route("v8net/v1/empresa-por-nome/{nome}")]
        public IEnumerable<BuscarEmpresasResumidoQueryResult> BuscarPorNome(string nome)
        {
            return _repository.BuscarPorNome(nome);
        }

        /// <summary>
        /// Retorna os dados de edição da tela pelo id.
        /// </summary>
        /// <param name="id">Id da empresa</param>
        /// <returns>Objeto contendo os dados resumidos das empresa.</returns>
        [HttpGet]
        [Route("v8net/v1/editar-empresa/{id}")]
        public EditarEmpresaQueryResult Editar(int id)
        {
            return _repository.Editar(id);
        }

        /// <summary>
        /// Retorna os dados resumidos da empresa pelo id.
        /// </summary>
        /// <param name="id">Nome da empresa</param>
        /// <returns>Objeto contendo os dados resumidos das empresa.</returns>
        [HttpGet]
        [Route("v8net/v1/empresa-por-id/{id}")]
        public BuscarEmpresasResumidoQueryResult BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }

        /// <summary>
        /// Cria uma nova empresa na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/nova-empresa")]
        public ICommandResult Criar([FromBody]CriarEmpresaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Atualiza uma empresa na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPut]
        [Route("v8net/v1/atualizar-empresa")]
        public ICommandResult Atualizar([FromBody]AtualizarEmpresaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui empresa na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-empresa")]
        public ICommandResult Excluir([FromBody]ExcluirEmpresaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }
    }
}
