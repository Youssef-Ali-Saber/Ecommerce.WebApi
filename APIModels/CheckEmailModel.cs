using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject_E_Commerce_.APIModels
{
    public class CheckEmailModel
    {
        public class Request
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;
        }
        public class Response
        {
            public bool Exists { get; set; }
        }
    }
}
