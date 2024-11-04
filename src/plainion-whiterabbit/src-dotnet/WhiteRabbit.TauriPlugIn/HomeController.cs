using TauriDotNetBridge.Contracts;

namespace WhiteRabbit.TauriPlugIn;

public class LogInInfo
{
    public string? User { get; set; }
    public string? Password { get; set; }
}

public class HomeController
{
    [RouteMethod]
    public RouteResponse Login(LogInInfo loginInfo)
    {
        return RouteResponse.Ok($"User '{loginInfo.User}' logged in successfully");
    }
}
