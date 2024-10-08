﻿using AgentsClient.Dto;
using AgentsClient.Enums;
using AgentsClient.Models;
using AgentsClient.ViewModel;
using System.Text.Json;
using System.Net.Http.Headers;

namespace AgentsClient.Service
{
    public class AgentService(
        IHttpClientFactory clientFactory,
        Authentication authentication
    ) : IAgentService
    {
        private readonly string baseUrl = "https://localhost:7083/Agents";
        public async Task<List<Agent>> GetAllAgents(string url = "")
        {
            if (authentication.Token == null)
            {
                throw new ArgumentNullException("No valid token");
            }
            var client = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{url}");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", authentication.Token);

            var Response = await client.SendAsync(request);

            if (Response.IsSuccessStatusCode)
            {
                var content = await Response.Content.ReadAsStringAsync();
                List<Agent>? agents = JsonSerializer.Deserialize<List<Agent>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                ) ?? [];
                return (agents);
            }
            return [];
        }
        public async Task<int> GetCountAgents()
        {
            var allAgents = await GetAllAgents();
            return allAgents.Count();
        }
        public async Task<int> GetCountActiveAgents()
        {
            var allAgents = await GetAllAgents();
            return allAgents.Where(a => a.Status == StatusAgentEnum.Active).Count();
        }
        public async Task<List<AgentVM>> GetAllAgentsVMAsync()
        {
            var allAgents = await GetAllAgents("/incloudMissions");

            return allAgents.Select(a => new AgentVM()
            {
                Id = a.Id,
                Image = a.Image,
                Nickname = a.Nickname,
                XPosition = a.XPosition,
                YPosition = a.YPosition,
                Status = a.Status,
                Duration = TimeSpan.FromHours(a.Missions.FirstOrDefault(m => m.Status == StatusMissionEnum.Assigned)?.TimeLeft ?? 0),
                AmountamountEliminations = a.Missions.Where(m => m.Status == StatusMissionEnum.Finished).Count(),
                Missions=a.Missions,
            }).ToList();
        }
        public async Task<AgentVM> GetAgentsVMByIdAsync(int id)
        {
            var agents = await GetAllAgentsVMAsync();
            return agents.FirstOrDefault(a => a.Id == id)
                ?? throw new Exception("agent is not found");
        }
        public async Task<List<(int X, int Y)>> GetAllAgentPositions()
        {
            var allAgents = await GetAllAgents();
            return allAgents.Select(a => (a.XPosition, a.YPosition)).ToList()
                ?? [];
        }

    }
}