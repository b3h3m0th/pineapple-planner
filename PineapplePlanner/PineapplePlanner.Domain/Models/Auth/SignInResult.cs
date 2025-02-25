namespace PineapplePlanner.Domain.Models.Auth
{
    public class SignInResult
    {
        public bool Success { get; set; }
        public FirebaseUser? User { get; set; }
        public string? Error { get; set; }
    }
}
