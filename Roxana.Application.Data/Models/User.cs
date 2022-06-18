using System.ComponentModel.DataAnnotations;
using Roxana.Application.Core.Enums.Membership;
using Roxana.Application.Data.Models.Base;

namespace Roxana.Application.Data.Models;


public class User : BaseEntity
{
    public UserType Type { get; set; }
    [MaxLength(64)]public string Username { get; set; }
    [MaxLength(64)]public string Password { get; set; }
    [MaxLength(128)]public string Salt { get; set; }
    [MaxLength(64)]public string FullName { get; set; }
    [MaxLength(32)]public string Timezone { get; set; }
    [MaxLength(128)]public string Email { get; set; }
    [MaxLength(16)]public string Phone { get; set; }

    public virtual ICollection<UserToken> Tokens { get; set; }
}