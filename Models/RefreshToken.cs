using Microsoft.EntityFrameworkCore;

namespace FirstWebApiProject_E_Commerce_.Models
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpOn { get; set; }
        public bool IsExpired => ExpOn <= DateTime.UtcNow;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedIn { get; set; }
        public bool IsActive => RevokedIn == null && !IsExpired;

    }
}
 