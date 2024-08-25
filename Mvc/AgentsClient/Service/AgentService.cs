using AgentsClient.Dto;
using AgentsClient.Enums;
using AgentsClient.Models;
using AgentsClient.ViewModel;
using System.Text.Json;

namespace AgentsClient.Service
{
    public class AgentService(IHttpClientFactory clientFactory) : IAgentService
    {
        private readonly string baseUrl = "https://localhost:7083/Agents";
        public async Task<List<Agent>> GetAllAgents()
        {
            var client = clientFactory.CreateClient();
            var result = await client.GetAsync($"{baseUrl}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<Agent>? agents = JsonSerializer.Deserialize<List<Agent>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                ) ?? [];
                return (agents);
            }
            return [];
        }
        public async Task<int> GetCountAgents()
        {
            var allAgents = await GetAllAgents();
            return allAgents.Count();
        }
        public async Task<int> GetCountActiveAgents()
        {
            var allAgents = await GetAllAgents();
            return allAgents.Where(a => a.Status == StatusAgentEnum.Active).Count();
        }
    }
}
