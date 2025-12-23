namespace Openbook.Wasm;

public static class HttpClientExtensions
{
    public static void SetJwtAuthorizationHeader(this HttpClient http, string token)
    {
        if (!http.DefaultRequestHeaders.Contains("Authorization"))
        {
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        else
        {
            http.DefaultRequestHeaders.Remove("Authorization");
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}