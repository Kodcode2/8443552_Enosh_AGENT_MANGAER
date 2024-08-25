using AgentsClient.Dto;

namespace AgentsClient.Service
{
    public interface IMissionService
    {
        Task<List<MissionDto>> GetAllMissionsAsync();
        Task<MissionDto> GetMissionAsync(int id);
        Task<bool> RunMissionAsync(int id);
    }
}
