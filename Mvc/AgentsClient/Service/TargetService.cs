using AgentsClient.Enums;
using AgentsClient.Models;
using System.Text.Json;

namespace AgentsClient.Service
{
    public class TargetService(IHttpClientFactory clientFactory) : ITargetService
    {
        private readonly string baseUrl = "https://localhost:7083/Targets";
        public async Task<List<Target>> GetAllTargets()
        {
            var client = clientFactory.CreateClient();
            var result = await client.GetAsync($"{baseUrl}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<Target>? targets = JsonSerializer.Deserialize<List<Target>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                ) ?? [];
                return (targets);
            }
            return [];
        }
        public async Task<int> GetCountTargets()
        {
            var allAgents = await GetAllTargets();
            return allAgents.Count();
        }
        public async Task<int> GetCountEliminatedTargets()
        {
            var allAgents = await GetAllTargets();
            return allAgents.Where(t => t.Status == StatusTargetEnum.Eliminated).Count();
        }
    }
}
