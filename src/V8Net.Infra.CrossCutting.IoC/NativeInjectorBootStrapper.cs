using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using V8Net.Domain.UsuarioBaseContext.Commands.Handlers;
using V8Net.Domain.UsuarioBaseContext.Repositories;
using V8Net.Domain.UsuarioBaseContext.Services;
using V8Net.Infra.CrossCutting.AspNetFilters;
using V8Net.Infra.Data.DataContext;
using V8Net.Infra.Data.UsuarioBase.Repositories;
using V8Net.Infra.Data.UsuarioBase.Services;

namespace V8Net.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            /////////////////////////////////////////////////////////////////////////////////////////
            // Registro de Dependências /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////
            // Modos de vida para um serviço que está sendo injetado:
            // Singleton:   Um objeto do serviço é criado e fornecido para todas as requisições. Assim, todas as requisições obtém o mesmo objeto;
            //    Escope:   Um objeto do serviço é criado para cada requisição. Dessa forma, cada requisição obtém uma nova instância do serviço;
            // Transient:   Um objeto do serviço é criado toda vez que um objeto for requisitado;
            /////////////////////////////////////////////////////////////////////////////////////////

            // Domain - Handlers / Repositories
            services.AddTransient<IUsuarioBaseRepository, UsuarioBaseRepository>();
            services.AddTransient<UsuarioBaseHandler, UsuarioBaseHandler>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<EmpresaHandler, EmpresaHandler>();
            services.AddTransient<ITelaRepository, TelaRepository>();
            services.AddTransient<TelaHandler, TelaHandler>();
            services.AddTransient<IAreaAtuacaoRepository, AreaAtuacaoRepository>();
            services.AddTransient<AreaAtuacaoHandler, AreaAtuacaoHandler>();

            // Domain - Services
            services.AddTransient<IEmailService, EmailService>();

            // Infra - Data
            services.AddScoped<V8NetDataContext, V8NetDataContext>();

            // Infra - Filters
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();
        }
    }
}
