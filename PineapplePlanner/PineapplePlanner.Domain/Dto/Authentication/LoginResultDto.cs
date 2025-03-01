namespace PineapplePlanner.Domain.Dto.Authentication
{
    public class LoginResultDto : AuthResultBase
    {
        public FirebaseUserDto? User { get; set; }
    }
}
