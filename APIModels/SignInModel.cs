using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FirstWebApiProject_E_Commerce_.APIModels
{
    public class SignInModel
    {

        public class Request
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;
            [Required]
            public string Password { get; set; } = null!;
        }

        public class Response
        {
            //public DateTime ExpIn { get; set; }
            public string? Token { get; set; }
            [JsonIgnore]
            public string? RefreshToken { get; set; }
            public DateTime RefreshTokenExpon { get; set; }
        }
    }
}