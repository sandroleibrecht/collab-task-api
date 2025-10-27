using CollabTaskApi.Application;
using CollabTaskApi.Infrastructure;
using CollabTaskApi.Shared;
using Serilog;

namespace CollabTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddApplication();
			builder.Services.AddInfrastructure(builder.Configuration, builder.Host);
			builder.Services.AddShared();

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
