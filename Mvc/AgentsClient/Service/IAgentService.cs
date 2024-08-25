using AgentsClient.Enums;
using AgentsClient.Models;
using AgentsClient.ViewModel;
using System.Text.Json;

namespace AgentsClient.Service
{
    public interface IAgentService
    {
        Task<List<Agent>> GetAllAgents(string url);
        Task<int> GetCountAgents();
        Task<int> GetCountActiveAgents();
        Task<List<AgentVM>> GetAllAgentsVMAsync();
        Task<AgentVM> GetAgentsVMByIdAsync(int id);
    }
}
