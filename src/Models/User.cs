namespace IDWM_TallerAPI.Src.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Rut { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Password { get; set; }
        public required bool Status { get; set; }
        
        public required int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}