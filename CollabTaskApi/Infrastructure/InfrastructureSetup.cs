using CollabTaskApi.Infrastructure.Background;
using CollabTaskApi.Infrastructure.Configuration;
using CollabTaskApi.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using Serilog;

namespace CollabTaskApi.Infrastructure
{
	public static class InfrastructureSetup
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, IHostBuilder host)
		{
			services.AddDatabase(config);
			services.AddHostedService<TokenCleanupService>();
			services.AddOptions(config);
			services.AddJwtAuthentication(config);
			services.AddSwaggerDocumentation();
			services.AddFluentValidation();
			host.ConfigureSerilog();
			services.AddAuthorization();

			return services;
		}

		public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
		{
			app.UseSerilogRequestLogging();

			return app;
		}

		private static void AddDatabase(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>( options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));
		}

		private static void AddFluentValidation(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<Program>();
		}

		private static void ConfigureSerilog(this IHostBuilder host)
		{
			host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
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

		private static void AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CollabTask API", Version = "v1" });

				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					BearerFormat = "JWT",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					Description = "Enter your AccessToken below.",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};

				c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
				c.AddSecurityRequirement(new OpenApiSecurityRequirement {{jwtSecurityScheme, Array.Empty<string>()}});
			});
		}
	}
}
