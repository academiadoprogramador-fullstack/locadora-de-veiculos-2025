using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocadoraDeVeiculos.Infraestrutura.Orm;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaInfraestruturaOrm(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkConfig(configuration);

        services.AddScoped<RepositorioFuncionarioEmOrm>();
        services.AddScoped<RepositorioGrupoVeiculosEmOrm>();
        services.AddScoped<RepositorioPlanoCobrancaEmOrm>();

        return services;
    }

    private static void AddEntityFrameworkConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration["SQL_CONNECTION_STRING"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("A variável SQL_CONNECTION_STRING não foi fornecida.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, (opt) => opt.EnableRetryOnFailure(3)));
    }
}
