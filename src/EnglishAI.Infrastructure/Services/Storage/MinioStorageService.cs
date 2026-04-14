using EnglishAI.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace EnglishAI.Infrastructure.Services.Storage;

public sealed class MinioStorageService : IFileStorageService
{
    private readonly IConfiguration _configuration;
    private readonly IMinioClient _minio;

    public MinioStorageService(IConfiguration configuration)
    {
        _configuration = configuration;

        var endpoint = _configuration["Storage:ConnectionString"] ?? "http://localhost:9000";
        var accessKey = _configuration["Storage:AccessKey"] ?? "minioadmin";
        var secretKey = _configuration["Storage:SecretKey"] ?? "minioadmin123";

        endpoint = endpoint.Replace("http://", "", StringComparison.OrdinalIgnoreCase)
            .Replace("https://", "", StringComparison.OrdinalIgnoreCase);

        _minio = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct)
    {
        var bucket = _configuration["Storage:BucketName"] ?? "englishai";
        await EnsureBucketAsync(bucket, ct);

        var putArgs = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await _minio.PutObjectAsync(putArgs, ct);
        return await GetUrlAsync(fileName);
    }

    public Task<string> GetUrlAsync(string fileName)
    {
        var baseUrl = _configuration["Storage:ConnectionString"] ?? "http://localhost:9000";
        var bucket = _configuration["Storage:BucketName"] ?? "englishai";
        return Task.FromResult($"{baseUrl.TrimEnd('/')}/{bucket}/{fileName}");
    }

    public async Task DeleteAsync(string fileName, CancellationToken ct)
    {
        var bucket = _configuration["Storage:BucketName"] ?? "englishai";
        var removeArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);

        await _minio.RemoveObjectAsync(removeArgs, ct);
    }

    private async Task EnsureBucketAsync(string bucket, CancellationToken ct)
    {
        var existsArgs = new BucketExistsArgs().WithBucket(bucket);
        if (await _minio.BucketExistsAsync(existsArgs, ct))
        {
            return;
        }

        var makeArgs = new MakeBucketArgs().WithBucket(bucket);
        await _minio.MakeBucketAsync(makeArgs, ct);
    }
}

