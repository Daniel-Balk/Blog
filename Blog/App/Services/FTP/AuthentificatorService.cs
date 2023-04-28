using Zhaobang.FtpServer.Authenticate;

namespace Blog.App.Services.FTP;

public class AuthenticatorService : IAuthenticator
{
    private readonly UserService UserService;
    
    public AuthenticatorService(UserService userService)
    {
        UserService = userService;
    }
    
    public bool Authenticate(string userName, string password)
    {
        try
        {
            var user = UserService.LoginUsernameSync(userName, password);
            
            if(user.IsAuthor | user.IsAdmin | user.IsSuperAdmin)
                return true;
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}