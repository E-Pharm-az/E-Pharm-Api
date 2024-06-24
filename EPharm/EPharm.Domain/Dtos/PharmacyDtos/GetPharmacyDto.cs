namespace EPharm.Domain.Dtos.PharmacyDtos;

public class GetPharmacyDto
{
    public int Id { get; set; }
    public string OwnerId { get; set; }
    public string TIN { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public int BuildingNumber { get; set; }
    public int Floor { get; set; }
    public int RoomNumber { get; set; }
}