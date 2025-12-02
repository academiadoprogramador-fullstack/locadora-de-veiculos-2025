using LocadoraDeVeiculos.Aplicacao;
using LocadoraDeVeiculos.Infraestrutura.Orm;
using LocadoraDeVeiculos.WebApi.Config.Http;
using LocadoraDeVeiculos.WebApi.Config.Orm;
using LocadoraDeVeiculos.WebApi.Config.Swagger;
using System.Text.Json.Serialization;

namespace LocadoraDeVeiculos.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCamadaInfraestruturaOrm(builder.Configuration);
        builder.Services.AddCamadaAplicacao(builder.Configuration);
        builder.Services.AddSwaggerConfig();

        builder.Services.ConfigureOptions<CorsConfig>().AddCors();

        builder.Services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.AplicarMigracoesOrm();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
