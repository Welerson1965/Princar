using MercadoLivre.Core.Domain.Interfaces.UoW;
using MercadoLivre.Infra.Data.Context;
using MercadoLivre.Infra.Data.External.MercadoLivre;
using MercadoLivre.Infra.Data.Repositories.Seguranca;
using MercadoLivre.Infra.Data.UoW;
using MercadoLivre.Seguranca.Domain.Interfaces.MercadoLivre;
using MercadoLivre.Seguranca.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MercadoLivre.WebApi
{
    public static class Setup
    {
        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MercadoLivre WebApi",
                    Version = "v1"
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"<b>JWT Autorização</b> <br/> 
                      Digite 'Bearer' [espaço] e em seguida seu token na caixa de texto abaixo.
                      <br/> <br/>
                      <b>Exemplo:</b> 'bearer 123456abcdefg...'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, string securityKey)
        {
            //Especifica o esquema usado para autenticação do Tipo Bearer
            //e define configurações como chave, algoritimo, validade, data expiração...
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "ispac",
                        ValidAudience = "ispac",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
                    };
                });
        }

        public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MercadoLivreContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        public static void ConfigureMediatorCoreDomain(this IServiceCollection services)
        {
            var assemblyCoreDomain = AppDomain.CurrentDomain.Load("MercadoLivre.Core.Domain");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyCoreDomain));
        }

        public static void ConfigureMediatorSegurancaDomain(this IServiceCollection services)
        {
            var assemblySegurancaDomain = AppDomain.CurrentDomain.Load("MercadoLivre.Seguranca.Domain");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblySegurancaDomain));
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddTransient<IRepositoryPrincarNotificacoes, RepositoryPrincarNotificacoes>();
        }
        public static void ConfigureMercadoLivreExterno(this IServiceCollection services)
        {
            services.AddTransient<IMercadoLivreApi, MercadoLivreApi>();
        }
    }
}
