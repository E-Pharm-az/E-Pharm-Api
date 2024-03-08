using System.ComponentModel.DataAnnotations;

namespace EPharm.Domain.Dtos.PharmaCompanyDtos;

public class CreatePharmaCompanyDto
{
    [Required]
    public string TIN { get; set; }
    [Required]
    public string CompanyName { get; set; }
    [Required]
    public string ContactEmail { get; set; }
    [Required]
    public string ContactPhone { get; set; }
    [Required]
    public string StreetAddress { get; set; }
    [Required]
    public string PostalCode { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Region { get; set; }
    [Required]
    public int BuildingNumber { get; set; }
    public int Floor { get; set; }
    public int RoomNumber { get; set; }
}
