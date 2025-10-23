using Microsoft.EntityFrameworkCore;
using FluentValidation;
using CollabTaskApi.Data;
using CollabTaskApi.Services;
using CollabTaskApi.Services.Interfaces;

namespace CollabTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
			
			// Service Layer
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IDeskService, DeskService>();
			builder.Services.AddScoped<IBoardService, BoardService>();
			builder.Services.AddScoped<IInviteService, InviteService>();

			// FluentValidation
			builder.Services.AddValidatorsFromAssemblyContaining<Program>();

			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
