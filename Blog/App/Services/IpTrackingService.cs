namespace Blog.App.Services;

public class IpTrackingService
{
    private readonly IHttpContextAccessor HttpContextAccessor;
    
    public IpTrackingService(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public string GetIpAddress()
    {
        if(HttpContextAccessor.HttpContext!.Request.Headers.ContainsKey("X-Real-IP")!)
            return HttpContextAccessor.HttpContext!.Request.Headers["X-Real-IP"]!;
        return HttpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }
}