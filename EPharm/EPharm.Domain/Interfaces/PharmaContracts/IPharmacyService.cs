using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyService
{
    public Task<IEnumerable<GetPharmacyDto>> GetAllPharmacyAsync();
    public Task<GetPharmacyDto?> GetPharmacyByIdAsync(int pharmaCompanyId);
    public Task InviteAsync(EmailDto emailDto);
    public Task<bool> VerifyInvitationAsync(string userId);
    public Task<GetPharmacyDto> Register(CreatePharmaDto createPharmaDto);
    public Task<GetPharmacyDto> CreateAsync(string userId, CreatePharmacyDto createPharmacyDto);
    public Task<bool> UpdateAsync(int id, CreatePharmacyDto pharmacyDto);
    public Task<bool> DeleteAsync(int pharmaCompanyId);
}
