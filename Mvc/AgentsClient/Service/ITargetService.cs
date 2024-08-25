using AgentsClient.Enums;
using AgentsClient.Models;
using System.Text.Json;

namespace AgentsClient.Service
{
    public interface ITargetService
    {
        Task<List<Target>> GetAllTargets();
        Task<int> GetCountTargets();
        Task<int> GetCountEliminatedTargets();
    }
}
