using AgentsRest.Models;
using AgentsRest.Dto;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<Mission>> GetAllMissionsWithAgentAndTargetAsync();
        Task<List<MissionDto>> GetAllMissionsDtoAsync();
        Task CreateMission(Agent agent);
        Task CreateMission(Target target);
        Task DeleteIrrelevantMissions();
        Task MoveAllActiveAgentsTowardsTargetAsync();
        Task RunMission(int id);
    }
}
