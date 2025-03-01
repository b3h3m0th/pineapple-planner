namespace PineapplePlanner.Domain.Dto.Authentication
{
    public class LogoutResultDto : AuthResultBase
    {
        public FirebaseUserDto? User { get; set; }
    }
}
