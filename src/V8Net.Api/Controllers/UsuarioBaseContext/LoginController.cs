using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using V8Net.Api.Security;
using V8Net.Domain.UsuarioBaseContext.Commands.Inputs;
using V8Net.Domain.UsuarioBaseContext.Commands.Outputs;
using V8Net.Domain.UsuarioBaseContext.Entities;
using V8Net.Domain.UsuarioBaseContext.Enums;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Domain.UsuarioBaseContext.ValueObjects;
using V8Net.Shared.Commands;

namespace V8Net.Api.Controllers.UsuarioBaseContext
{
    public class LoginController : Controller
    {
        private UsuarioBase _usuarioBase;
        private readonly IUsuarioBaseRepository _repository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;

        public LoginController(
            IUsuarioBaseRepository repository,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            _repository = repository;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        /// <summary>
        /// Validação e autenticação de um usuário cadastrado na base de dados.
        /// </summary>
        /// <returns>Classe: Success(boolean), Message(string) e Data(object).</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("v8net/v1/autenticacao")]
        public ICommandResult Post([FromBody]AutenticarUsuarioBaseCommand command)
        {
            var login = new LoginVO(command.Usuario, command.Senha);
            if (login.Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", login.Notifications);

            _usuarioBase = _repository.UsuarioBase(login.Usuario, login.Senha);
            if (_usuarioBase == null)
                return new CommandResult(false, "Falha ao autenticar. Usuário e/ou senha inválidos", new { });

            var senhaTemporaria = _repository.BuscarUsuarioSenhaTemporaria(_usuarioBase.Id);
            if (senhaTemporaria != null)
                if (DateTime.Now.Subtract(senhaTemporaria.DataCadastro).Days >= 1)
                    if (senhaTemporaria.Ativo == EBoolean.True)
                        return new CommandResult(false, "Sua senha temporária expirou. Favor, Solicitar uma nova senha.", new { });

            return Autenticar;
        }

        public ICommandResult Autenticar
        {
            get
            {
                var dataCriacao = DateTime.Now;
                var dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_tokenConfigurations.SECONDS);
                var handler = new JwtSecurityTokenHandler();

                var identity = new ClaimsIdentity(
                    new GenericIdentity(_usuarioBase.Login.Usuario, "TokenAuth"),
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, _usuarioBase.Login.Usuario)
                    }
                );

                return new CommandResult(true, "Usuário autenticado",
                    new
                    {
                        userId = _usuarioBase.Id,
                        user = _usuarioBase.Login.Usuario,
                        authenticated = true,
                        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = handler.WriteToken(handler.CreateToken(new SecurityTokenDescriptor
                        {
                            Issuer = _tokenConfigurations.ISSUER,
                            Audience = _tokenConfigurations.AUDIENCE,
                            SigningCredentials = _signingConfigurations.SigningCredentials,
                            Subject = identity,
                            NotBefore = dataCriacao,
                            Expires = dataExpiracao
                        })),
                        message = "OK"
                    });
            }
        }
    }
}
