using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Interfaces.CommonContracts;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EPharm.Domain.Services.Common;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(CreateEmailDto emailDto)
    {
        var client = new RestClient(configuration["ResendConfig:BaseUrl"]!);
        var request = new RestRequest("/emails", Method.Post);
        
        request.AddHeader("Authorization", $"Bearer {configuration["ResendConfig:Key"]}");
        request.AddHeader("Content-Type", "application/json");

        request.AddJsonBody(new
        {
            from = configuration["SmtpConfig:Sender"],
            to = emailDto.Email,
            subject = emailDto.Subject,
            html  = emailDto.Message
        });
        
        await client.ExecuteAsync(request);
    }
}
