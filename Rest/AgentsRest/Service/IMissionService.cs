using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<Mission>> GetAllMissionsWithAgentAndTargetAsync();
        Task CreateMission(Agent agent);
        Task CreateMission(Target target);
        Task DeleteIrrelevantMissions();
        Task MoveAllActiveAgentsTowardsTargetAsync();
        Task RunMission(int id);
    }
}
