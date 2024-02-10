using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstWebApiProject_E_Commerce_.APIModels
{
    public class SignUpModel
    {
        public class Request
        {
            [Required]
            [MaxLength(50)]
            public string FirstName { get; set; } = null!;
            [Required]
            [MaxLength(50)]
            public string LastName { get; set; } = null!;
            [Required]
            [MaxLength(50)]
            public string UserName { get; set; } = null!;
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;
            [Required]
            [MinLength(8)]
            [MaxLength(20)]
            public string Password { get; set; } = null!;
        }
        public class Response
        {
            public string? JWToken { get; set; }
            public string? Email { get; set; }
            public string? UserName { get; set; }
            public List<string>? Roles { get; set; }
            //public DateTime? ExpOn { get; set; }
            [JsonIgnore]
            public string? RefreshToken { get; set; }
            public DateTime RefreshTokenExpOn {  get; set; }
        }
    }
}
