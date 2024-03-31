using EPharm.Domain.Dtos.EmailDto;
using EPharm.Domain.Interfaces.CommonContracts;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EPharm.Domain.Services.CommonServices;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(CreateEmailDto emailDto)
    {
        var client = new RestClient("https://api.resend.com");
        var request = new RestRequest("/emails", Method.Post);
        
        request.AddHeader("Authorization", $"Bearer {configuration["ResendApi:Key"]}");
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
