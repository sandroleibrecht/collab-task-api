using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using FluentValidation;
using Serilog;
using Scalar.AspNetCore;
using CollabTask.Api.Infrastructure.Data;
using CollabTask.Api.Infrastructure.Background;
using CollabTask.Api.Infrastructure.Configuration;

namespace CollabTask.Api.Infrastructure
{
	public static class InfrastructureSetup
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, IHostBuilder host)
		{
			host.ConfigureSerilog();
			services.AddDatabase(config);
			services.AddHostedService<TokenCleanupService>();
			services.AddOptions(config);
			services.AddJwtAuthentication(config);
			services.AddOpenApiConfig();
			services.AddFluentValidation();
			services.AddAuthorization();

			return services;
		}

		public static IApplicationBuilder UseInfrastructure(this WebApplication app)
		{
			app.UseSerilogRequestLogging();

			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.MapScalarApiReference(options =>
				{
					options
						.WithTitle("Collab Task API")
						.WithTheme(ScalarTheme.Purple)
						.WithClassicLayout()
						.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
				});
			}

			return app;
		}

		private static void ConfigureSerilog(this IHostBuilder host)
		{
			host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
		}

		private static void AddDatabase(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>( options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));
		}

		private static void AddFluentValidation(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<Program>();
		}

		private static void AddOptions(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<JwtOptions>(config.GetSection("Jwt"));
		}

		private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
		{
			var jwtConfig = config.GetSection("Jwt");
			var key = jwtConfig["Key"];
			if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("JWT Key is missing or too short.");
			if (key.Length < 16) throw new InvalidOperationException("JWT Key is too short (>= 16 chars required).");

			var issuer = jwtConfig["Issuer"];
			var audience = jwtConfig["Audience"];

			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true,
						ValidIssuer = issuer,
						ValidAudience = audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
						ClockSkew = TimeSpan.FromSeconds(30)
					};
				});
		}

		private static void AddOpenApiConfig(this IServiceCollection services)
		{
			services.AddOpenApi(options =>
			{
				options.AddDocumentTransformer((document, context, cancellationToken) =>
				{
					document.Components ??= new OpenApiComponents();
					document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

					document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
					{
						Type = SecuritySchemeType.Http,
						Scheme = "bearer",
						BearerFormat = "JWT",
						Description = "Insert token here (excluding 'Bearer')"
					};

					document.Security = [
						new OpenApiSecurityRequirement
						{
							[new OpenApiSecuritySchemeReference("Bearer", document)] = []
						}
					];

					return Task.CompletedTask;
				});
			});
		}
	}
}
