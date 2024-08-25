using AgentsClient.Enums;
using AgentsClient.Models;
using System.Text.Json;

namespace AgentsClient.Service
{
    public interface IAgentService
    {
        Task<List<Agent>> GetAllAgents();
        Task<int> GetCountAgents();
        Task<int> GetCountActiveAgents();
    }
}
