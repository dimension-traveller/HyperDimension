namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface IPasswordHashService
{
    public string HashPassword(string password, string salt);
    public bool VerifyPassword(string password, string salt, string passwordHash);
}
