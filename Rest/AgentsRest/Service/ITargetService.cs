using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ITargetService
    {
        Task<IdDto> CreateTargetAsync(TargetDto target);
        Task<List<Target>> GetAllTargetsAsync();
        Task PinPositionAsync(int id, PositionDto position);
        Task MoveTargetAsync(int id, DirectionDto direction);
    }
}
