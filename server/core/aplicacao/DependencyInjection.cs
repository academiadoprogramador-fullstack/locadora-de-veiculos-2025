using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocadoraDeVeiculos.Aplicacao;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaAplicacao(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        var licenseKey = configuration["LUCKYPENNY_LICENSE_KEY"];

        if (string.IsNullOrWhiteSpace(licenseKey))
            throw new Exception("A variável LUCKYPENNY_LICENSE_KEY não foi fornecida.");

        services.AddMediatR(config =>
        {
            config.LicenseKey = licenseKey;
            config.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}
