namespace Travelo.Application.DTOs.Auth
{
    public class updateUserProfileDTORes
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string?  ProfileImageHash { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
    }
}
