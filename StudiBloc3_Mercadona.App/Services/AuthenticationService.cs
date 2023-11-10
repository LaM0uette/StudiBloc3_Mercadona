namespace StudiBloc3_Mercadona.App.Services;

public class AuthenticationService
{
    public bool IsAuthenticated { get; private set; }

    public Task<bool> Login(string username, string password)
    {
        if (username == "root" && password == "manager")
        {
            IsAuthenticated = true;
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public void Logout()
    {
        IsAuthenticated = false;
    }
}