using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.WebApp.Components.Modal;

public class NewProductModalBase : ComponentBase
{
    #region Statements

    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<Product> OnSubmit { get; set; }
    
    protected Product Product { get; set; } = new();

    #endregion

    protected Task HandleSubmit() => OnSubmit.InvokeAsync(Product);
    protected Task HandleClose() => OnClose.InvokeAsync(null);
}