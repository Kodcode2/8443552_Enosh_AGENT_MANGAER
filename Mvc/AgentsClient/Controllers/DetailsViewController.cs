using AgentsClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentsClient.Controllers
{
    public class DetailsViewController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string baseUrl = "https://localhost:7083/Targets";
        public async Task<IActionResult> Index()
        {
            var result = await clientFactory.CreateClient().GetAsync(baseUrl);
            if (result.IsSuccessStatusCode)
            {
                
                var content = await result.Content.ReadAsStringAsync();
                List<Target>? targets = JsonSerializer.Deserialize<List<Target>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                )
                    ?? new  List<Target>();
                return View(targets);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
