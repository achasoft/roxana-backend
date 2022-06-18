using System.ComponentModel.DataAnnotations.Schema;
using Roxana.Application.Data.Models.Base;

namespace Roxana.Application.Data.Models;

public class UserToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }

    [ForeignKey("UserId")]public virtual User User { get; set; }
}