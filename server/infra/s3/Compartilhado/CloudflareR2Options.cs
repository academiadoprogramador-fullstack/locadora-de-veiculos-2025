namespace LocadoraDeVeiculos.Infraestrutura.S3.Compartilhado;

public sealed class CloudflareR2Options
{
    public string AccountId { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;   // https://<ACCOUNT_ID>.r2.cloudflarestorage.com
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}
