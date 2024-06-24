using System.ComponentModel.DataAnnotations;

namespace EPharm.Infrastructure.Entities.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
