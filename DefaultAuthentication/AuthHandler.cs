using System.Text;

namespace DefaultAuthentication;

public class AuthHandler
{
    private readonly RequestDelegate _next;
    private readonly string _relm;

    public AuthHandler(RequestDelegate next, string relm)
    {
        _next = next;
        _relm = relm;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var header = context.Request.Headers["Authorization"].ToString();
        var endcodedCreds = header.Substring(6); //Basic userid:password
        var creds = Encoding.UTF8.GetString(Convert.FromBase64String(endcodedCreds));
        string[] uspw = creds.Split(":");
        if (uspw.Length <= 1)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var user = uspw[0];
        var password = uspw[1];
        //check if the user exists

        await _next(context); 

    }
}

