
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Custom;
using TareaPractica5Unidad5.DB;
using TareaPractica5Unidad5.Services.UsuariosServices;

namespace TareaPractica5Unidad5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<TareaPractica5Context>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("Practica5"));
            });

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "PEC");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
