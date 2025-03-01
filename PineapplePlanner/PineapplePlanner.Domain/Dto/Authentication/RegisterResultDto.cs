namespace PineapplePlanner.Domain.Dto.Authentication
{
    public class RegisterResultDto : AuthResultBase
    {
        public FirebaseUserDto? User { get; set; }
    }
}
