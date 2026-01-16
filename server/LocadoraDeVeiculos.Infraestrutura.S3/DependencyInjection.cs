using Amazon.Runtime;
using Amazon.S3;
using LocadoraDeVeiculos.Infraestrutura.S3.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.S3.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LocadoraDeVeiculos.Infraestrutura.S3;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaInfraestruturaS3(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddCloudflareR2Config(configuration);

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CloudflareR2Options>>().Value;

            var credentials = new BasicAWSCredentials(options.AccessKeyId, options.SecretAccessKey);

            return new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = options.ServiceUrl,
            });
        });

        services.AddScoped<RepositorioR2FileStorage>();

        return services;
    }

    private static IServiceCollection AddCloudflareR2Config(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CloudflareR2Options>(
            configuration.GetSection("CLOUDFLARE_R2_CREDENTIALS"));

        return services;
    }
}
