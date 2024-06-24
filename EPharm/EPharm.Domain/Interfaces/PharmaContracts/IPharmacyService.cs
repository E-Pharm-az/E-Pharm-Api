using EPharm.Domain.Dtos.PharmaCompanyDtos;
using EPharm.Domain.Dtos.UserDto;

namespace EPharm.Domain.Interfaces.PharmaContracts;

public interface IPharmacyService
{
    public Task<IEnumerable<GetPharmaCompanyDto>> GetAllPharmaCompaniesAsync();
    public Task<GetPharmaCompanyDto?> GetPharmaCompanyByIdAsync(int pharmaCompanyId);
    public Task InvitePharmaAsync(EmailDto emailDto);
    public Task InitializePharmaAsync(string userId, string token, CreatePharmaDto createPharmaDto);
    public Task CreatePharmaAsync(CreatePharmaDto createPharmaDto);
    public Task<bool> UpdatePharmaCompanyAsync(int id, CreatePharmaCompanyDto pharmaCompanyDto);
    public Task<bool> DeletePharmaCompanyAsync(int pharmaCompanyId);
}