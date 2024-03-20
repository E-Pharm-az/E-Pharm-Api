using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using EPharm.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services;

public class ProductImageService : IProductImageService
{
    private readonly IConfiguration _configuration;
    private readonly AmazonS3Client _s3Client;

    public ProductImageService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        var credentials = new BasicAWSCredentials(
            _configuration["AwsConfig:AccessKey"],
            _configuration["AwsConfig:SecretKey"]
            );
        
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
        };
        
        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<string> UploadProductImageAsync(IFormFile image)
    {
        await using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);

        var request = new PutObjectRequest
        {
            BucketName = _configuration["AwsConfig:ImageBucket"],
            Key = "product-images/" + Guid.NewGuid(),
            InputStream = memoryStream,
            ContentType = "image/jpeg"
        };
        var response = await _s3Client.PutObjectAsync(request);
        
        if (response.HttpStatusCode == HttpStatusCode.OK)
            return $"https://{_configuration["AwsConfig:ImageBucket"]}.s3.eu-central-1.amazonaws.com/{request.Key}";

        throw new InvalidOperationException("Failed to upload image");
    }

    public async Task<bool> DeleteProductImageAsync(string imageUrl)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _configuration["AwsConfig:ImageBucket"],
            Key = imageUrl.Split("/").Last()
        };
        
        var response = await _s3Client.DeleteObjectAsync(request);
        
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}
