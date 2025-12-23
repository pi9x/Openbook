using Microsoft.JSInterop;

namespace Openbook.Wasm.Services;

public class AuthStateService
{
    private readonly IJSRuntime _js;
    public string? UserName { get; private set; }
    public string? UserEmail { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(UserName);
    public event Action? AuthStateChanged;

    public AuthStateService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task LoadAsync()
    {
        UserName = await _js.InvokeAsync<string>("localStorage.getItem", "userName");
        UserEmail = await _js.InvokeAsync<string>("localStorage.getItem", "userEmail");
        AuthStateChanged?.Invoke();
    }

    public async Task SetAuthAsync(string? userName, string? userEmail)
    {
        UserName = userName;
        UserEmail = userEmail;
        await _js.InvokeVoidAsync("localStorage.setItem", "userName", userName ?? "");
        await _js.InvokeVoidAsync("localStorage.setItem", "userEmail", userEmail ?? "");
        AuthStateChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "jwt");
        await _js.InvokeVoidAsync("localStorage.removeItem", "userName");
        await _js.InvokeVoidAsync("localStorage.removeItem", "userEmail");
        UserName = null;
        UserEmail = null;
        AuthStateChanged?.Invoke();
    }
}
