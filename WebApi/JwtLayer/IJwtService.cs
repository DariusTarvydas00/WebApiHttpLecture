namespace UserInterface.JwtLayer;

public interface IJwtService
{
    string GetJWT(string username, string role);
}