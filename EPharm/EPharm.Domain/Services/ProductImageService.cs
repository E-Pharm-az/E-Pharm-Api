using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AutoMapper;
using EPharm.Domain.Dtos.ProductImageDto;
using EPharm.Domain.Interfaces;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using Microsoft.Extensions.Configuration;

namespace EPharm.Domain.Services;

public class ProductImageService(IConfiguration configuration, IProductImageRepository productImageRepository, IMapper mapper) : IProductImageService
{
    public async Task<bool> UploadProductImageAsync(CreateProductImageDto productImageDto)
    {
        var credentials = new BasicAWSCredentials(
            configuration["AwsConfig:AccessKey"],
            configuration["AwsConfig:SecretKey"]
            );
        
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
        };
        
        await using var memoryStream = new MemoryStream();
        await productImageDto.Image.CopyToAsync(memoryStream);

        using var client = new AmazonS3Client(credentials, config);
        var request = new PutObjectRequest
        {
            BucketName = configuration["AwsConfig:ImageBucket"],
            Key = "product-images/" + Guid.NewGuid(),
            InputStream = memoryStream,
            ContentType = "image/jpeg"
        };
        var response = await client.PutObjectAsync(request);
        
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            var productImage = mapper.Map<ProductImage>(productImageDto);
            productImage.ImageUrl = request.Key;
            await productImageRepository.InsertAsync(productImage);
        }

        return false;
    }
}
