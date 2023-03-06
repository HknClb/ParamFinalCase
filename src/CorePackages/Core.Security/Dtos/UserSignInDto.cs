namespace Core.Security.Dtos
{
    public class UserSignInDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
