using AgentsClient.Enums;
using AgentsClient.Models;
using System.Text.Json;

namespace AgentsClient.Service
{
    public interface ITargetService
    {
        Task<List<Target>> GetAllTargetsAsync();
        Task<int> GetCountTargetsAsync();
        Task<int> GetCountEliminatedTargetsAsync();
        Task<List<(int X, int Y)>> GetAllTargetPositions();
    }
}
