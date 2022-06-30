using System.Text;
using System.Text.Json.Serialization;
using Influence.Service.Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using Serilog;
using StaticValues = Influence.Service.Helper.StaticValues;

namespace Influence.Service.Configuration;

public static class Configurator
{
    public static void UseConfigurator(this IServiceCollection serviceCollection, ConfigurationManager configuration,string apiName="Admin")
    {
        var jwtTokenConfig = configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
        serviceCollection.AddSingleton(jwtTokenConfig);
        serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.UseIocLoader();

        serviceCollection.AddRouting(options => options.LowercaseUrls = true);

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).Enrich.FromLogContext()
            .CreateLogger();

        StaticValues.Url = configuration.GetConnectionString("ApiUrl");
        StaticValues.FileUrl = configuration.GetConnectionString("FileUrl");
        StaticValues.FilePath = configuration.GetConnectionString("FilePath");



        serviceCollection
            .AddScoped<Service.Configuration.IAuthenticatedMemberService,
                Service.Configuration.AuthenticatedMemberService>();

        serviceCollection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromDays(360),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = jwtTokenConfig.Issuer,
                ValidAudience = jwtTokenConfig.Audience,
            };
        });
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Influence "+apiName,
                Description = $"Influence {apiName} Api",
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    Array.Empty<string>()
                }
            });
        });

        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection.AddCors();

        serviceCollection.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
        serviceCollection.AddMvc().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        serviceCollection.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        serviceCollection.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();

        serviceCollection.Configure<MongoDBSettings>(
            configuration.GetSection("MongoDB"));
        var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);
    }
}