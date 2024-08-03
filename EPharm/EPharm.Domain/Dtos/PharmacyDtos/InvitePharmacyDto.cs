namespace EPharm.Domain.Dtos.PharmacyDtos;

public class InvitePharmacyDto : UserDto.EmailDto
{
    public string PharmacyName { get; set; }
}
