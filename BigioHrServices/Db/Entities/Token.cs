using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FFIBot.CMS.Db.Entities;

[Table("token")]
public class Token
{
    [Key]
    [MaxLength(255)]
    [Column("id_token")]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("id_user")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("type")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Column("issued_at")]
    public DateTime IssuedAt { get; set; } = DateTime.Now;

    public virtual User? User { get; set; }
}
