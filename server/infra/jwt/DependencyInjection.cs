using LocadoraDeVeiculos.Infraestrutura.Jwt.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LocadoraDeVeiculos.Infraestrutura.Jwt;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaInfraestruturaJwt(this IServiceCollection services)
    {
        services.AddScoped<AccessTokenProvider>();
        services.AddScoped<RefreshTokenProvider>();

        return services;
    }
}
