using AgentsClient.Enums;
using AgentsClient.Models;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text.Json;

namespace AgentsClient.Service
{
    public class TargetService(
        IHttpClientFactory clientFactory,
        Authentication authentication
    ) : ITargetService
    {
        private readonly string baseUrl = "https://localhost:7083/Targets";
        public async Task<List<Target>> GetAllTargetsAsync()
        {
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", authentication.Token);

            var Response = await client.SendAsync(request);
            if (Response.IsSuccessStatusCode)
            {
                var content = await Response.Content.ReadAsStringAsync();
                List<Target>? targets = JsonSerializer.Deserialize<List<Target>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                ) ?? [];
                return (targets);
            }
            return [];
        }
        public async Task<int> GetCountTargetsAsync()
        {
            var allAgents = await GetAllTargetsAsync();
            return allAgents.Count();
        }
        public async Task<int> GetCountEliminatedTargetsAsync()
        {
            var allAgents = await GetAllTargetsAsync();
            return allAgents.Where(t => t.Status == StatusTargetEnum.Eliminated).Count();
        }

        public async Task<List<(int X, int Y)>> GetAllTargetPositions()
        {
            var allAgents = await GetAllTargetsAsync();
            return allAgents.Select(t => (t.XPosition, t.YPosition)).ToList()
                ?? [];
        }
    }
}
