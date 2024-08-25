using AgentsClient.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AgentsClient.Service
{
    public class MissionService(IHttpClientFactory clientFactory) : IMissionService
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

        public async Task<MissionDto> GetMissionAsync(int id)
        {
            var allMisions = await GetAllMissionsAsync();
            return allMisions.FirstOrDefault(m => m.Id == id)
                ?? throw new Exception("The mission does not exist");
        }
        public async Task<MissionDto> RunMissionAsync(int id)
        {
            var mission = GetMissionAsync(id);
            var client = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/{id}");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer"/*, authentication.Token*/);


            var respons = await client.SendAsync(request);
            if (!respons.IsSuccessStatusCode)
            {
                throw new Exception("The operation was not successful");
            }
            return await mission;
        }
    }
}
