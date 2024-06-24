using EPharm.Domain.Dtos.PharmacyDtos;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyService
{
    public Task<IEnumerable<GetPharmacyDto>> GetAllPharmacyAsync();
    public Task<GetPharmacyDto?> GetPharmacyByIdAsync(int pharmaCompanyId);
    public Task InvitePharmacyAsync(EmailDto emailDto);
    public Task InitializePharmacyAsync(string userId, string token, CreatePharmaDto createPharmaDto);
    public Task CreatePharmacyAsync(CreatePharmaDto createPharmaDto);
    public Task<bool> UpdatePharmacyAsync(int id, CreatePharmacyDto pharmacyDto);
    public Task<bool> DeletePharmacyAsync(int pharmaCompanyId);
}