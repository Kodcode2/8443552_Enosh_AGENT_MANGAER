using AgentsClient.Dto;
using AgentsClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace AgentsClient.Service
{
    public class LoginService(
        IHttpClientFactory clientFactory, 
        Authentication authentication
    ) : ILoginService
    {
        private readonly string loginApi = "https://localhost:7083/Login";
        public async Task LoginAsync()
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, loginApi);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new LoginDto() { Id = "MVCServer" }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                TokenDto? token = JsonSerializer.Deserialize<TokenDto>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                if (token == null)
                {
                    throw new Exception("You didn't get a token");
                }
                authentication.Token = token.Token;
            }
        }
    }
}
