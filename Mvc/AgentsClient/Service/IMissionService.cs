using AgentsClient.Dto;
using AgentsClient.Enums;
using AgentsClient.ViewModel;

namespace AgentsClient.Service
{
    public interface IMissionService
    {
        Task<List<MissionDto>> GetAllMissionsAsync();
        Task<MissionDto> GetMissionByIdAsync(int id);
        Task<bool> RunMissionAsync(int id);
        Task<int> GetCountMissionsAsync();
        Task<int> GetCountAssignedMissionsAsync();
        Task<Statistics> GetStatisticsAsync();
    }
}
