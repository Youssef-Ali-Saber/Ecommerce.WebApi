using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject_E_Commerce_.APIModels
{
    public class AddRoleModel
    {
        public class Requset
        {
            [Required]
            public string userId { get; set; } = null!;
            [Required]
            public string roleName { get; set; } = null!;
        }

    }
}
