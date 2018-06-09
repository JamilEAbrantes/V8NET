using FluentValidator;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Handlers
{
    public class EmpresaHandler :
        Notifiable,
        ICommandHandler<CriarEmpresaCommand>,
        ICommandHandler<AtualizarEmpresaCommand>,
        ICommandHandler<ExcluirEmpresaCommand>
    {
        private readonly IEmpresaRepository _repository;

        public EmpresaHandler(IEmpresaRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(CriarEmpresaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            if (_repository.EmpresaExistente(command.Id))
                AddNotification("Empresa", $"Código de empresa já cadastrado na base de dados. Código informado: { command.Id }");

            if (_repository.EmpresaExistente(command.Nome))
                AddNotification("Empresa", $"Nome de empresa já cadastrado na base de dados. Nome informado: { command.Nome }");

            var empresa = new Empresa(command.Id, command.Nome, command.Fantasia, command.Telefone, command.TipoEmpresa,
                command.Cgc9, command.Cgc4, command.Cgc2);

            AddNotifications(empresa);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Salvar(empresa);

            return new CommandResult(true, "Empresa cadastrada com sucesso", new
            {
                Id = empresa.Id,
                Nome = empresa.Nome
            });
        }

        public ICommandResult Handle(AtualizarEmpresaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var empresa = _repository.Empresa(command.Id);
            if (empresa == null)
                return new CommandResult(false, $"A empresa não existe na base de dados. Código informado: { command.Id }", new { });

            if (command.Nome != empresa.Nome)
                if (_repository.EmpresaExistente(command.Nome))
                    AddNotification("Empresa", $"Nome de empresa já cadastrado na base de dados. Nome informado: { command.Nome }");

            empresa.AtribuirEmpresa(command.Nome, command.Fantasia, command.Telefone, command.TipoEmpresa, command.Cgc9, command.Cgc4, command.Cgc2);

            AddNotifications(empresa);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Atualizar(empresa);

            return new CommandResult(true, "Empresa atualizada com sucesso", new
            {
                Id = empresa.Id,
                Nome = empresa.Nome
            });
        }
       
        public ICommandResult Handle(ExcluirEmpresaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var empresa = _repository.Empresa(command.Id);
            if (empresa == null)
                return new CommandResult(false, $"A empresa não existe na base de dados. Código informado: { command.Id }", new { });

            _repository.Excluir(command.Id);

            return new CommandResult(true, "Empresa excluída com sucesso", new
            {
                Id = empresa.Id,
                Fantasia = empresa.Fantasia
            });
        }
    }
}
