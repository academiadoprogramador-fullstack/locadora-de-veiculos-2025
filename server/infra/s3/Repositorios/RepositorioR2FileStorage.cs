using Amazon.S3;
using Amazon.S3.Model;
using LocadoraDeVeiculos.Infraestrutura.S3.Compartilhado;
using Microsoft.Extensions.Options;

namespace LocadoraDeVeiculos.Infraestrutura.S3.Repositorios;

public sealed class RepositorioR2FileStorage(IAmazonS3 s3, IOptions<CloudflareR2Options> options)
{
    private readonly CloudflareR2Options _options = options.Value;

    public async Task<string> UploadAsync(
        Stream stream,
        string contentType,
        string key,
        CancellationToken cancellationToken = default
    )
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        await s3.PutObjectAsync(putRequest, cancellationToken);

        return key;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key
        };

        await s3.DeleteObjectAsync(deleteRequest, cancellationToken);
    }
}