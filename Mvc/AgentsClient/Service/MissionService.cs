using AgentsClient.Dto;
using AgentsClient.Enums;
using AgentsClient.ViewModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AgentsClient.Service
{
    public class MissionService(
        IHttpClientFactory clientFactory,
        IAgentService agentService,
        ITargetService targetService
    ) : IMissionService
    {
        private readonly string baseUrl = "https://localhost:7083/Missions";
        public async Task<List<MissionDto>> GetAllMissionsAsync()
        {
            var client = clientFactory.CreateClient();
            var result = await client.GetAsync(baseUrl);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<MissionDto>? missions = JsonSerializer.Deserialize<List<MissionDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                ) ?? [];
                return (missions);
            }
            return [];
        }

        public async Task<MissionDto> GetMissionByIdAsync(int id)
        {
            var allMisions = await GetAllMissionsAsync();
            return allMisions.FirstOrDefault(m => m.Id == id)
                ?? throw new Exception("The mission does not exist");
        }
        public async Task<bool> RunMissionAsync(int id)
        {
            var client = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/{id}");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer"/*, authentication.Token*/);


            var respons = await client.SendAsync(request);
            if (!respons.IsSuccessStatusCode)
            {
               return false;
            }
            return  true;
        }

        public async Task<int> GetCountMissionsAsync()
        {
            var allMissions = await GetAllMissionsAsync();
            return allMissions.Count();
        }
        public async Task<int> GetCountAssignedMissionsAsync()
        {
            var allMissions = await GetAllMissionsAsync();
            return allMissions.Where(m => m.status == StatusMissionEnum.Assigned).Count();
        }

        public async Task<Statistics> GetStatisticsAsync()
        {
            int totalAgents = await agentService.GetCountAgents(),
                allActiveAgents = await agentService.GetCountActiveAgents(),
                totalTargets = await targetService.GetCountTargetsAsync(),
                allEliminatedTargets = await targetService.GetCountEliminatedTargetsAsync();

            var g= new Statistics()
            {
                TotalAgents = totalAgents,
                AllActiveAgents = allActiveAgents,
                TotalTargets = totalTargets,
                AllEliminatedTargets = allEliminatedTargets,
                TotalMissions = await GetCountMissionsAsync(),
                AllAssignedMissions = await GetCountAssignedMissionsAsync(),
                RelationOfAgentsToTargets = (totalTargets) /Math.Max(totalAgents,1),
                RatioOfAgentsThatCanBeTeamedToTargetsAgainstTargets =
                (totalTargets - allEliminatedTargets) / Math.Max(totalAgents - allActiveAgents,1),
            };
            return g;
        }
    }
}