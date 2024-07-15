namespace Qhr.Server.Models;


public class User
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public string? PasswordHash { get; set; }
}