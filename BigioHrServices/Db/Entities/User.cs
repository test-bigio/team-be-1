using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFIBot.CMS.Db.Entities;

[Table("user")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User
{
    [Key]
    [MaxLength(50)]
    [Column("id_user")]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("address")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Required]
    [Column("is_verified")]
    public Boolean IsVerified { get; set; } = false;

    [Column("profile_picture")]
    public string? ProfilePicture { get ; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("role")]
    public string Role { get; set; } = string.Empty;

    public virtual ICollection<Token>? Tokens { get; set; }
}
