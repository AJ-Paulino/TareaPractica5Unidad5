
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Custom;
using TareaPractica5Unidad5.DB;
using TareaPractica5Unidad5.Services.UsuariosServices;
using CFDB;
using TareaPractica5Unidad5.Services;
using Microsoft.Extensions.Configuration;

namespace TareaPractica5Unidad5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //Conexión a la base de datos
            builder.Services.AddDbContext<TareaPractica5Context>(op =>            
                op.UseSqlServer(builder.Configuration.GetConnectionString("Practica5"))
            );

            //Configuración de JWT, encriptar contraseña
            builder.Services.AddSingleton<Utilidades>();

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                };
            });

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<CFDB.Practica5Context>(options =>            
                options.UseSqlServer(builder.Configuration.GetConnectionString("Practica5"))
            );

            builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();

            var key = builder.Configuration.GetValue<string>("JWT:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key!);

            var app = builder.Build();

            /*
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Practica5Context>();
                context.Database.Migrate();
            }*/

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

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
