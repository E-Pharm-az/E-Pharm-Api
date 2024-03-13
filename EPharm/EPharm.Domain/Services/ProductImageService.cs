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

public class ProductImageService : IProductImageService
{
    private readonly IConfiguration _configuration;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;
    private readonly AmazonS3Client _s3Client;

    public ProductImageService(IConfiguration configuration, IProductImageRepository productImageRepository, IMapper mapper)
    {
        _configuration = configuration;
        _productImageRepository = productImageRepository;
        _mapper = mapper;
        
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

    public async Task<bool> UploadProductImageAsync(CreateProductImageDto productImageDto)
    {
        await using var memoryStream = new MemoryStream();
        await productImageDto.Image.CopyToAsync(memoryStream);

        var request = new PutObjectRequest
        {
            BucketName = _configuration["AwsConfig:ImageBucket"],
            Key = "product-images/" + Guid.NewGuid(),
            InputStream = memoryStream,
            ContentType = "image/jpeg"
        };
        var response = await _s3Client.PutObjectAsync(request);
        
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            var productImage = _mapper.Map<ProductImage>(productImageDto);
            productImage.ImageUrl = request.Key;
            productImage.ImageUrl = $"https://{_configuration["AwsConfig:ImageBucket"]}.s3.eu-central-1.amazonaws.com/{request.Key}";
            await _productImageRepository.InsertAsync(productImage);
        }

        return false;
    }

    public async Task<bool> DeleteProductImageAsync(int id)
    {
        var productImage = await _productImageRepository.GetByIdAsync(id);
        if (productImage is null) return false;

        var request = new DeleteObjectRequest
        {
            BucketName = _configuration["AwsConfig:ImageBucket"],
            Key = productImage.ImageUrl.Split("/").Last()
        };
        
        var response = await _s3Client.DeleteObjectAsync(request);
        
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            _productImageRepository.Delete(productImage);
            await _productImageRepository.SaveChangesAsync();
            
            return true;
        }

        return false;
    }
}
