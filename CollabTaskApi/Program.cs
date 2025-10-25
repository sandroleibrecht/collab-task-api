using CollabTaskApi.Data;
using CollabTaskApi.Helpers.Auth;
using CollabTaskApi.Helpers.Auth.Interfaces;
using CollabTaskApi.Mappers;
using CollabTaskApi.Mappers.Interfaces;
using CollabTaskApi.Options;
using CollabTaskApi.Services;
using CollabTaskApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CollabTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			// Swagger
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

			// DB
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
			
			// Service Layer
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IDeskService, DeskService>();
			builder.Services.AddScoped<IBoardService, BoardService>();
			builder.Services.AddScoped<IInviteService, InviteService>();
			builder.Services.AddScoped<IJwtService, JwtService>();
			
			// Helpers
			builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

			// FluentValidation
			builder.Services.AddValidatorsFromAssemblyContaining<Program>();

			// Mappers
			builder.Services.AddScoped<IUserMapper, UserMapper>();
			builder.Services.AddScoped<IBoardMapper, BoardMapper>();
			builder.Services.AddScoped<IErrorMapper, ErrorMapper>();

			// JWT
			builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
			
			var jwtConfig = builder.Configuration.GetSection("Jwt");
			
			var key = jwtConfig["Key"];
			if (string.IsNullOrEmpty(key))
				throw new InvalidOperationException("JWT Key is missing or too short.");
			
			if (key.Length < 16)
				throw new InvalidOperationException("JWT Key is too short (>= 16 chars required).");

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

            app.UseHttpsRedirection();
			
			app.UseAuthentication();
            app.UseAuthorization();
            
			app.MapControllers();

            app.Run();
        }
    }
}
