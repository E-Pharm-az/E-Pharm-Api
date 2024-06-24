using EPharm.Domain.Dtos.EmailDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IEmailSender
{
    public Task SendEmailAsync(CreateEmailDto emailDto);
}