using FluentValidator;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Handlers
{
    public class TelaHandler :
        Notifiable,
        ICommandHandler<CriarTelaCommand>,
        ICommandHandler<AtualizarTelaCommand>,
        ICommandHandler<ExcluirTelaCommand>
    {
        private readonly ITelaRepository _telaRepository;
        private readonly IAreaAtuacaoRepository _areaAtuacaoRepository;

        public TelaHandler(ITelaRepository repository, IAreaAtuacaoRepository areaAtuacaoRepository)
        {
            _telaRepository = repository;
            _areaAtuacaoRepository = areaAtuacaoRepository;
        }

        public ICommandResult Handle(CriarTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            if (_telaRepository.TelaExistente(command.Titulo))
                AddNotification("Empresa", $"Nome de tela já cadastrado na base de dados. Nome informado: { command.Titulo }");

            var areaAtuacao = _areaAtuacaoRepository.AreaAtuacao(command.IdAreaAtuacao);
            if (areaAtuacao == null)
                AddNotification("IdAreaAtuacao", $"A área de atuação não existe na base de dados. Código informado: { command.IdAreaAtuacao }");

            var tela = new Tela(areaAtuacao, command.Titulo, command.Descricao, command.Link);

            AddNotifications(tela, areaAtuacao);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _telaRepository.Salvar(tela);

            return new CommandResult(true, "Tela cadastrada com sucesso", new
            {
                Area = areaAtuacao.Titulo,
                Titulo = tela.Titulo
            });
        }

        public ICommandResult Handle(AtualizarTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            var areaAtuacao = _areaAtuacaoRepository.AreaAtuacao(command.IdAreaAtuacao);
            if (areaAtuacao == null)
                return new CommandResult(false, $"A área de atuação não existe na base de dados. Código informado: { command.Id }", new { });

            var tela = _telaRepository.Tela(command.Id);
            if (tela == null)
                return new CommandResult(false, $"A tela não existe na base de dados. Código informado: { command.Id }", new { });

            if (command.Titulo != tela.Titulo)
                if (_telaRepository.TelaExistente(command.Titulo))
                    AddNotification("Titulo", $"Título já cadastrado na base de dados. Título informado: { command.Titulo }");

            tela.AtribuirTela(command.Titulo, command.Descricao, command.Link);
            tela.AtribuirAreaAtuacao(areaAtuacao);

            if (command.Ativo.Equals(EBoolean.False))
                tela.Desativar();
            else
                tela.Ativar();

            AddNotifications(tela, areaAtuacao);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _telaRepository.Atualizar(tela);

            return new CommandResult(true, "Tela atualizada com sucesso", new
            {
                Area = areaAtuacao.Titulo,
                Titulo = tela.Titulo
            });
        }
        
        public ICommandResult Handle(ExcluirTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            var tela = _telaRepository.Tela(command.Id);
            if (tela == null)
                return new CommandResult(false, "A tela não existe na base de dados", new { });

            _telaRepository.Excluir(tela.Id);

            return new CommandResult(true, "Tela excluída com sucesso", new
            {
                Id = tela.Id,
                Titulo = tela.Titulo
            });
        }
    }
}
