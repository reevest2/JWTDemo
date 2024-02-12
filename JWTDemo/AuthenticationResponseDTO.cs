namespace JWTDemo;

public class AuthenticationResponseDTO
{
    public string Token { get; set; }
    public UserDTO UserDTO { get; set; }
    public bool IsSuccessful { get; set; }
}