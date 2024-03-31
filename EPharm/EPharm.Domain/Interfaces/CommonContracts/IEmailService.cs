using EPharm.Domain.Dtos.EmailDto;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IEmailService
{
    public Task SendEmailAsync(CreateEmailDto emailDto);
}
