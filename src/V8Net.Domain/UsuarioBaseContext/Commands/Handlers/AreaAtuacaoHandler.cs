using FluentValidator;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Handlers
{
    public class AreaAtuacaoHandler :
        Notifiable,
        ICommandHandler<CriarAreaAtuacaoCommand>,
        ICommandHandler<AtualizarAreaAtuacaoCommand>,
        ICommandHandler<ExcluirAreaAtuacaoCommand>
    {
        private readonly IAreaAtuacaoRepository _repository;

        public AreaAtuacaoHandler(IAreaAtuacaoRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(CriarAreaAtuacaoCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            if (_repository.AreaAtuacaoExistente(command.Titulo))
                AddNotification("Titulo", $"Área de atuação já cadastrada na base de dados. Título informado: { command.Titulo }");

            var areaAtuacao = new AreaAtuacao(command.Titulo, command.Descricao);

            AddNotifications(areaAtuacao);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Salvar(areaAtuacao);

            return new CommandResult(true, "Área de atuação cadastrada com sucesso", new
            {
                Titulo = areaAtuacao.Titulo
            });
        }

        public ICommandResult Handle(AtualizarAreaAtuacaoCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            var areaAtuacao = _repository.AreaAtuacao(command.Id);
            if (areaAtuacao == null)
                return new CommandResult(false, $"A àrea de atuação não existe na base de dados. Código informado: { command.Id }", new { });

            if (command.Titulo != areaAtuacao.Titulo)
                if (_repository.AreaAtuacaoExistente(command.Titulo))
                    AddNotification("Titulo", $"Área de atuação já cadastrada na base de dados. Título informado: { command.Titulo }");

            areaAtuacao.AtribuirAreaAtuacao(command.Titulo, command.Descricao);

            if (command.Ativo.Equals(EBoolean.False))
                areaAtuacao.Desativar();
            else
                areaAtuacao.Ativar();

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Atualizar(areaAtuacao);

            return new CommandResult(true, "Área de atuação atualizada com sucesso", new
            {
                Id = areaAtuacao.Id,
                Titulo = areaAtuacao.Titulo
            });
        }
        
        public ICommandResult Handle(ExcluirAreaAtuacaoCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            var areaAtuacao = _repository.AreaAtuacao(command.Id);
            if (areaAtuacao == null)
                return new CommandResult(false, $"A área de atuação não existe na base de dados. Código informado: { command.Id }", new { });

            _repository.Excluir(areaAtuacao.Id);

            return new CommandResult(true, "Área de atuação excluída com sucesso", new
            {
                Id = areaAtuacao.Id,
                Titulo = areaAtuacao.Titulo
            });
        }
    }
}
