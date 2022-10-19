namespace MusicLib.API.Models.Login;

public class LoginUserInfo
{
    public int ID { get; set; }

    public string UserName { get; set; }
    
    public string DisplayName { get; set; }

    public string Role { get; set; }

    public string Email { get; set; }
}