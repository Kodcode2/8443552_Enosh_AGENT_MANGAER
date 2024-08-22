using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task CreateMission(Agent agent);
        Task CreateMission(Target target);
        Task DeleteIrrelevantMissions();
        Task MoveAllActiveAgentsTowardsTarget();


    }
}
