using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.UserDto;
using EPharm.Infrastructure.Entities.Identity;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyService
{
    public Task<IEnumerable<GetPharmacyDto>> GetAllPharmacyAsync();
    public Task<GetPharmacyDto?> GetPharmacyByIdAsync(int pharmaCompanyId);
    public Task InviteAsync(InvitePharmacyDto invitePharmacyDto);
    public Task<AppIdentityUser> VerifyInvitationAsync(string userId);
    public Task<GetPharmacyDto> Register(CreatePharmaDto createPharmaDto);
    public Task<GetPharmacyDto> CreateAsync(string userId, CreatePharmacyDto createPharmacyDto);
    public Task<bool> UpdateAsync(int id, CreatePharmacyDto pharmacyDto);
    public Task<bool> DeleteAsync(int pharmacyId);
}
