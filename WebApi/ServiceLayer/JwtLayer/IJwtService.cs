namespace WebApi.ServiceLayer.JwtLayer;

public interface IJwtService
{
    public string GetJWT(string username);
}