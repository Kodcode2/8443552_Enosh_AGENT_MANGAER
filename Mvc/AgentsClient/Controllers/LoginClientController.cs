using AgentsClient.Dto;
using AgentsClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace AgentsClient.Controllers
{
    public class LoginClientController(IHttpClientFactory clientFactory, Authentication authentication) : Controller
    {
        private readonly string loginApi = "https://localhost:7083/Login";

        public IActionResult Index() => View();
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, loginApi);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new LoginDto() { Id = "ControlManager" }),
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
                if( token == null )
                {
                    throw new Exception("You didn't get a token");
                }
                authentication.Token = token.Token;
                return RedirectToAction("index", "Home");
            }
            return View("AuthError");
        }
    }
}