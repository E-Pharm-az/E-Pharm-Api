using System.ComponentModel.DataAnnotations;

namespace EPharm.Infrastructure.Context.Entities.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
