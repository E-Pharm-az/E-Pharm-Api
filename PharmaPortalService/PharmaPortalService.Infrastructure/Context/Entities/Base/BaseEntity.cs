using System.ComponentModel.DataAnnotations;

namespace PharmaPortalService.Infrastructure.Context.Entities.Base;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
