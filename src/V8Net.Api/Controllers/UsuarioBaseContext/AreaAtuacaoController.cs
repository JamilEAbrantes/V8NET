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
    public class AreaAtuacaoController : Controller
    {
        private readonly IAreaAtuacaoRepository _repository;
        private readonly AreaAtuacaoHandler _handler;

        public AreaAtuacaoController(IAreaAtuacaoRepository repository, AreaAtuacaoHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        /// <summary>
        /// Retorna uma lista resumida contendo todas as áreas de atuação.
        /// </summary>
        /// <returns>Objeto contendo os dados resumidos das áreas de atuação.</returns>
        [HttpGet]
        [Route("v8net/v1/atuacoes")]
        public IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarTodas()
        {
            return _repository.BuscarTodos();
        }

        /// <summary>
        /// Retorna uma lista resumida das áreas de atuação pelo nome.
        /// </summary>
        /// <param name="nome">Nome da empresa</param>
        /// <returns>Objeto contendo os dados resumidos das áreas de atuação.</returns>
        [HttpGet]
        [Route("v8net/v1/atuacao-por-nome/{nome}")]
        public IEnumerable<BuscarAreaAtuacaoResumidoQueryResult> BuscarPorNome(string nome)
        {
            return _repository.BuscarPorNome(nome);
        }

        /// <summary>
        /// Retorna os dados resumidos da área de atuação pelo id.
        /// </summary>
        /// <param name="id">Id área de atuação</param>
        /// <returns>Objeto contendo os dados resumidos das empresa.</returns>
        [HttpGet]
        [Route("v8net/v1/atuacao-por-id/{id}")]
        public BuscarAreaAtuacaoResumidoQueryResult BuscarPorId(int id)
        {
            return _repository.BuscarPorId(id);
        }

        /// <summary>
        /// Retorna os dados para edição da área de atuação pelo id.
        /// </summary>
        /// <param name="id">Id área de atuação</param>
        /// <returns>Objeto contendo os dados para edição da área de atuação.</returns>
        [HttpGet]
        [Route("v8net/v1/editar-atuacao/{id}")]
        public EditarAreaAtuacaoQueryResult Editar(int id)
        {
            return _repository.Editar(id);
        }

        /// <summary>
        /// Cria uma nova área de atuação na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [Route("v8net/v1/nova-atuacao")]
        public ICommandResult Criar([FromBody]CriarAreaAtuacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Atualiza uma área de atuação na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPut]
        [Route("v8net/v1/atualizar-atuacao")]
        public ICommandResult Atualizar([FromBody]AtualizarAreaAtuacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        /// <summary>
        /// Exclui uma área de atuação na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpDelete]
        [Route("v8net/v1/excluir-atuacao")]
        public ICommandResult Excluir([FromBody]ExcluirAreaAtuacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }
    }
}
