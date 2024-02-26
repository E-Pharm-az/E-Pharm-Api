using EPharm.Infrastructure.Context.Entities.Base;

namespace EPharm.Infrastructure.Context.Entities;

public class Address : BaseEntity
{
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public int BuildingNumber { get; set; }
    public int Floor { get; set; }
    public int RoomNumber { get; set; } 
    public int PharmaCompanyId { get; set; }
    public PharmaCompany PharmaCompany { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
