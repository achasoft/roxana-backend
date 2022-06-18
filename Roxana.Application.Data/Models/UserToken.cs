using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Roxana.Application.Data.Models.Base;

namespace Roxana.Application.Data.Models;

public class UserToken : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
    [MaxLength(64)]public string Hash { get; set; }
    [MaxLength(512)]public string Token { get; set; }

    [ForeignKey("UserId")]public virtual User User { get; set; }
}