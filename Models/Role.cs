using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Role
{   
    [Key]
    [Column("role_id")]
    public int? RoleId { get; set; }
    [Column("role_name")]
    public string? RoleName { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
