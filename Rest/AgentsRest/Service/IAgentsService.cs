using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IAgentsService
    {
        Task<IdDto> CreateAgentAsync(AgentDto agent);
        Task<List<Agent>> GetAllAgentsAsync();
        Task PinPositionAsync(int id, PositionDto position);
        Task MoveAgentAsync(int id, DirectionDto direction);
    }
}
