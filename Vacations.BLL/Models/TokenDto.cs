namespace Vacations.BLL.Models
{
    public class TokenDto
    {
        public string Token { get; }
        public string Role { get; }

        public TokenDto(string token, string role)
        {
            Token = token;
            Role = role;
        }
    }
}
