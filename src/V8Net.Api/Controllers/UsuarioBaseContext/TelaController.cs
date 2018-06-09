using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using V8Net.Domain.UsuarioBaseContext.Commands.Handlers;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Queries;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Api.Controllers.UsuarioBaseContext
{
    public class TelaController : Controller
    {
        private readonly ITelaRepository _repository;
        private readonly TelaHandler _handler;

        public TelaController(ITelaRepository repository, TelaHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        /// <summary>
        /// Retorna uma lista resumida contendo todas as telas cadastrados na base de dados.
        /// </summary>
        /// <returns>Objeto contendo os dados resumidos da tela.</returns>
        [HttpGet]
        [Route("v8net/v1/telas")]
        public IEnumerable<BuscarTelaResumidoQueryResult> BuscarTodos()
        {
            return _repository.BuscarTodos();
        }

        /// <summary>
        /// Retorna os dados para edição da tela pelo id.
        /// </summary>
        /// <param name="id">Id da tela</param>
        /// <returns>Objeto contendo os dados para edição da tela.</returns>
        [HttpGet]
        [Route("v8net/v1/editar-tela/{id}")]
        public EditarTelaQueryResult Editar(int id)
        {
            return _repository.Editar(id);
        }

        /// <summary>
        /// Retorna os dados resumidos da tela pelo id.
        /// </summary>
        /// <param name="id">Id da tela</param>
        /// <returns>Objeto contendo os dados resumidos da tela.</returns>
        [HttpGet]
        [Route("v8net/v1/tela-por-id/{id}")]
        public BuscarTelaResumidoQueryResult BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }

        /// <summary>
        /// Retorna os dados resumidos da tela pelo nome.
        /// </summary>
        /// <param name="nome">Id da tela</param>
        /// <returns>Objeto contendo os dados resumidos da tela.</returns>
        [HttpGet]
        [Route("v8net/v1/tela-por-nome/{nome}")]
        public IEnumerable<BuscarTelaResumidoQueryResult> BuscarPorNome(string nome)
        {
            return _repository.BuscarPorNome(nome);
        }

        /// <summary>
        /// Cria uma nova tela na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/nova-tela")]
        public ICommandResult Criar([FromBody]CriarTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Atualiza uma tela na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPut]
        [Route("v8net/v1/atualizar-tela")]
        public ICommandResult Atualizar([FromBody]AtualizarTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui uma tela na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-tela")]
        public ICommandResult Excluir([FromBody]ExcluirTelaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }
    }
}
