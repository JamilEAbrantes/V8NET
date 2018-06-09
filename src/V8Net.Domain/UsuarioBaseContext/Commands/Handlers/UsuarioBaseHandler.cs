using FluentValidator;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Domain.UsuarioBaseContext.Services;
using V8Net.Domain.UsuarioBaseContext.ValueObjects;
using V8Net.Shared.Commands;

namespace V8Net.Domain.UsuarioBaseContext.Commands.Handlers
{
    public class UsuarioBaseHandler :
        Notifiable,
        ICommandHandler<CriarUsuarioBaseCommand>,
        ICommandHandler<AtualizarUsuarioBaseCommand>,
        ICommandHandler<CriarUsuarioEmpresaCommand>,
        ICommandHandler<CriarUsuarioAtuacaoCommand>,
        ICommandHandler<CriarUsuarioTelaCommand>,
        ICommandHandler<AtualizarUsuarioTelaCommand>,
        ICommandHandler<ExcluirUsuarioBaseCommand>,
        ICommandHandler<RecuperarSenhaCommand>,
        ICommandHandler<ExcluirUsuarioEmpresaCommand>,
        ICommandHandler<ExcluirUsuarioAtuacaoCommand>,
        ICommandHandler<ExcluirUsuarioTelaCommand>
    {
        private readonly IUsuarioBaseRepository _repository;
        private readonly IEmailService _service;

        public UsuarioBaseHandler(IUsuarioBaseRepository repository, IEmailService service)
        {
            _repository = repository;
            _service = service;
        }

        #region --> Usuário Base (principal)

        public ICommandResult Handle(CriarUsuarioBaseCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            if (_repository.UsuarioExistente(command.Usuario))
                AddNotification("Usuario", $"Nome de usuário já cadastrado na base de dados. Nome informado: { command.Usuario }");

            if (_repository.DocumentoExistente(command.Documento))
                AddNotification("CPF/CNPJ", $"CPF/CNPJ já cadastrado na base de dados. Doc. informado: { command.Documento }");

            if (_repository.EmailExistente(command.Email))
                AddNotification("E-mail", $"E-mail já cadastrado na base de dados. E-mail informado: { command.Email }");

            var login = new LoginVO(command.Usuario, command.Senha);
            var email = new EmailVO(command.Email);
            var documento = new DocumentoVO(command.Documento);
            var usuario = new UsuarioBase(
                login, 
                email, 
                documento, 
                command.PerfilAcesso, 
                command.ImpressoraZebra);

            AddNotifications(usuario, login, email, documento);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Salvar(usuario);

            return new CommandResult(true, "Usuário cadastrado com sucesso", new
            {
                Usuario = usuario.Login.Usuario,
                Documento = usuario.Documento.Documento
            });
        }

        public ICommandResult Handle(AtualizarUsuarioBaseCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.Id);
            if (usuario == null)
                return new CommandResult(false, $"O usuario não existe na base de dados. Id de usuário informado: { command.Id }", new { });

            if (command.Usuario != usuario.Login.Usuario)
                if (_repository.UsuarioExistente(command.Usuario))
                    AddNotification("Usuario", $"Nome de usuário já cadastrado na base de dados. Nome informado: { command.Usuario }");

            if (command.Documento != usuario.Documento.Documento)
                if (_repository.DocumentoExistente(command.Documento))
                    AddNotification("CPF/CNPJ", $"CPF/CNPJ já cadastrado na base de dados. Doc. informado: { command.Documento }");

            if (command.Email != usuario.Email.Email)
                if (_repository.EmailExistente(command.Email))
                    AddNotification("E-mail", $"E-mail já cadastrado na base de dados. E-mail informado: { command.Email }");

            var login = new LoginVO(command.Usuario);
            var email = new EmailVO(command.Email);
            var documento = new DocumentoVO(command.Documento);

            if (usuario.Login.Senha != command.Senha)
                login = new LoginVO(command.Usuario, command.Senha);
            else
                login.AtribuirSenha(command.Senha);

            if (command.Ativo.Equals(EBoolean.False))
                usuario.Desativar();
            else
                usuario.Ativar();

            usuario.AtribuirUsuario(
                login, 
                email, 
                documento, 
                command.PerfilAcesso, 
                command.ImpressoraZebra);

            AddNotifications(usuario, login, email, documento);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Atualizar(usuario);

            return new CommandResult(true, "Usuário atualizado com sucesso", new
            {
                Id = usuario.Id,
                Usuario = usuario.Login.Usuario,
                Documento = usuario.Documento.Documento
            });
        }

        public ICommandResult Handle(ExcluirUsuarioBaseCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.Id);
            if (usuario == null)
                return new CommandResult(false, $"O usuario não existe na base de dados. Id de usuário informado: { command.Id }", new { });

            _repository.Excluir(command.Id);

            return new CommandResult(true, "Usuário excluído com sucesso", new
            {
                Id = usuario.Id,
                Usuario = usuario.Login.Usuario,
                Documento = usuario.Documento.Documento
            });
        }

        #endregion

        #region --> Usuário Login

        public ICommandResult Handle(RecuperarSenhaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.Email);
            if (usuario == null)
                return new CommandResult(false, $"O usuario não existe na base de dados. E-mail informado: { command.Email }", new { });

            var senhaNova = usuario.Login.GerarSenha();
            var senhaNovaCriptografada = usuario.Login.EncriptarSenha(senhaNova);

            _repository.ConfigurarSenhaTemporaria(usuario.Id, senhaNovaCriptografada);
            _service.RecuperarSenha(usuario.Email.Email, senhaNova);

            return new CommandResult(true, $"Um e-mail contendo uma nova senha temporária (válido por apenas 1 dia) foi enviado para o endereço: { usuario.Email.Email }", new { });
        }

        #endregion

        #region --> Usuário Empresa

        public ICommandResult Handle(CriarUsuarioEmpresaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.IdUsuario);
            if (usuario == null)
                return new CommandResult(false, "O usuario não existe na base de dados", new { });

            var empresa = _repository.Empresa(command.IdEmpresa);
            if (empresa == null)
                return new CommandResult(false, "A empresa não existe na base de dados", new { });

            if (_repository.UsuarioEmpresaExistente(command.IdUsuario, command.IdEmpresa))
                return new CommandResult(false, $"O usuário { usuario.Id } - { usuario.Login.Usuario } já está relacionado com a empresa { empresa.Id } - { empresa.Fantasia } { empresa.Cgc9 }/{ empresa.Cgc4 }-{ empresa.Cgc2 }", new { });

            var usuarioEmpresa = new UsuarioEmpresa(command.IdUsuario, command.IdEmpresa);

            AddNotifications(usuarioEmpresa);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.SalvarUsuarioEmpresa(usuarioEmpresa);

            return new CommandResult(true, "Empresa cadastrada com sucesso", new
            {
                Usuario = usuario.Login.Usuario,
                Empresa = $"{ empresa.Id } - { empresa.Fantasia } { empresa.Cgc9 }/{ empresa.Cgc4 }-{ empresa.Cgc2 }"
            });
        }

        public ICommandResult Handle(ExcluirUsuarioEmpresaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuarioEmpresa = _repository.UsuarioEmpresaExistente(command.Id);
            if (!usuarioEmpresa)
                return new CommandResult(false, "Empresa não existe na base de dados", new { });

            _repository.ExcluirUsuarioEmpresa(command.Id);

            return new CommandResult(true, "Empresa excluída com sucesso", new { });
        }

        #endregion

        #region --> Usuario Atuação

        public ICommandResult Handle(CriarUsuarioAtuacaoCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.IdUsuario);
            if (usuario == null)
                return new CommandResult(false, "O usuario não existe na base de dados", new { });

            var atuacao = _repository.AreaAtuacao(command.IdAreaAtuacao);
            if (atuacao == null)
                return new CommandResult(false, "A área de atuação não existe na base de dados", new { });

            if (_repository.UsuarioAtuacaoExistente(command.IdUsuario, command.IdAreaAtuacao))
                return new CommandResult(false, $"O usuário { usuario.Id } - { usuario.Login.Usuario } já está relacionado com a àrea { atuacao.Id } - { atuacao.Titulo }", new { });

            var usuarioAtuacao = new UsuarioAreaAtuacao(usuario, atuacao);

            AddNotifications(usuarioAtuacao);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.SalvarUsuarioAtuacao(usuarioAtuacao);

            return new CommandResult(true, "Área de atuação cadastrada com sucesso", new
            {
                Usuario = usuario.Login.Usuario,
                Atuacao = $"{ atuacao.Id } - { atuacao.Titulo }"
            });
        }

        public ICommandResult Handle(ExcluirUsuarioAtuacaoCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuarioAtuacao = _repository.UsuarioAtuacaoExistente(command.Id);
            if (!usuarioAtuacao)
                return new CommandResult(false, "Área de atuação não existe na base de dados", new { });

            _repository.ExcluirUsuarioAtuacao(command.Id);

            return new CommandResult(true, "Área de atuação excluída com sucesso", new { });
        }

        #endregion

        #region --> Usuário Tela

        public ICommandResult Handle(CriarUsuarioTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.IdUsuario);
            if (usuario == null)
                return new CommandResult(false, "O usuario não existe na base de dados", new { });

            var tela = _repository.Tela(command.IdTela);
            if (tela == null)
                return new CommandResult(false, "A tela não existe na base de dados", new { });

            if (_repository.UsuarioTelaExistente(command.IdUsuario, command.IdTela))
                return new CommandResult(false, $"O usuário { usuario.Id } - { usuario.Login.Usuario } já está relacionado com a tela { tela.Id } - { tela.Titulo }", new { });

            var usuarioTela = new UsuarioTela(usuario, tela);

            AddNotifications(usuarioTela);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.SalvarUsuarioTela(usuarioTela);

            return new CommandResult(true, "Pemissão cadastrada com sucesso", new
            {
                Usuario = usuario.Login.Usuario,
                Tela = $"{ tela.Id } - { tela.Titulo }"
            });
        }

        public ICommandResult Handle(AtualizarUsuarioTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            var usuario = _repository.UsuarioBase(command.IdUsuario);
            if (usuario == null)
                return new CommandResult(false, "O usuario não existe na base de dados", new { });

            var tela = _repository.Tela(command.IdTela);
            if (tela == null)
                return new CommandResult(false, "A tela não existe na base de dados", new { });

            var usuarioTela = _repository.UsuarioTela(command.Id, usuario, tela);
            if (usuarioTela == null)
                return new CommandResult(false, $"O usuário { usuario.Id } - { usuario.Login.Usuario } não está relacionado com a tela { tela.Id } - { tela.Titulo }", new { });

            usuarioTela.AtribuirUsuarioPemissoes(command.Incluir, command.Atualizar, command.Excluir, command.Consultar);

            AddNotifications(usuarioTela);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.AtualizarUsuarioTela(usuarioTela);

            return new CommandResult(true, "Pemissão atualizada com sucesso", new
            {
                Usuario = usuario.Login.Usuario,
                Tela = $"{ tela.Id } - { tela.Titulo }"
            });
        }

        public ICommandResult Handle(ExcluirUsuarioTelaCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", command.Notifications);

            if (!_repository.UsuarioTelaExistente(command.Id))
                return new CommandResult(false, "A permissão não existe na base de dados", new { });

            _repository.ExcluirUsuarioTela(command.Id);

            return new CommandResult(true, "Pemissão excluída com sucesso", new { });
        }

        #endregion
    }
}
