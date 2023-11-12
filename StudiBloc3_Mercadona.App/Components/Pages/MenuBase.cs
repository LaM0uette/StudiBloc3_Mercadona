using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.App.Services;

namespace StudiBloc3_Mercadona.App.Components.Pages;

public class MenuBase : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private AuthenticationService authService { get; init; } = default!;
    
    protected string? Username;
    protected string? Password;

    protected async Task Login()
    {
        if (Username is null || Password is null) 
        {
            return;
        }
        
        var result = await authService.Login(Username, Password);
        if (!result)
        {
            return;
        }
        
        NavigationManager.NavigateTo("/catalog");
    }

    protected async Task ContinueAsGuest()
    {
        await authService.Logout();
        NavigationManager.NavigateTo("/catalog");
    }
}