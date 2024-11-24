using Microsoft.AspNetCore.Identity;

namespace IDWM_TallerAPI.Src.Models
{
    public class User : IdentityUser<int>
    {
        public required string Rut { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required bool Status { get; set; }
    }
}