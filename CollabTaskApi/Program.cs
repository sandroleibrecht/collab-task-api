using CollabTaskApi.Data;
using CollabTaskApi.Options;
using FluentValidation;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CollabTaskApi.Services;
using CollabTaskApi.Helpers;

namespace CollabTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddServices();
			builder.Services.AddHelpers();
			builder.Services.AddOptions(builder.Configuration);
			builder.Services.AddDb(builder.Configuration);


			// serilog
			builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

			// swagger
			builder.Services.AddSwaggerGen(c =>
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
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{ jwtSecurityScheme, Array.Empty<string>() }
				});
			});

			// FluentValidation
			builder.Services.AddValidatorsFromAssemblyContaining<Program>();
			
			// jwt
			var jwtConfig = builder.Configuration.GetSection("Jwt");
			var key = jwtConfig["Key"];
			if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("JWT Key is missing or too short.");
			if (key.Length < 16) throw new InvalidOperationException("JWT Key is too short (>= 16 chars required).");
			var issuer = jwtConfig["Issuer"];
			var audience = jwtConfig["Audience"];
			builder.Services
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

			builder.Services.AddAuthorization();

			var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.UseSerilogRequestLogging();
			app.UseHttpsRedirection();
			app.UseAuthentication();
            app.UseAuthorization();
			app.MapControllers();

            app.Run();
        }
    }
}
