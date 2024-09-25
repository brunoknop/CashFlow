using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public abstract class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public CashFlowClassFixture(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    public async Task<HttpResponseMessage> DoPost(string requestUri, object content, string token = "", string culture = "en")
    {
        AuthorizeWithToken(token);
        SetCultureInfo(culture);
        return await _httpClient.PostAsJsonAsync(requestUri, content);
    }

    private void AuthorizeWithToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void SetCultureInfo(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
