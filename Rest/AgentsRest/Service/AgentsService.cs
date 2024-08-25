using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Enums;
using AgentsRest.Models;
using AgentsRest.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace AgentsRest.Service
{
    public class AgentsService(
        ApplicationDbContext context, 
        IServiceProvider serviceProvider
    ) : IAgentsService
    {
        private IMissionService _missionService = serviceProvider.GetRequiredService<IMissionService>();
        public async Task<IdDto> CreateAgentAsync(AgentDto agent)
        {
            Agent model = new()
            {
                Image = agent.PhotoUrl,
                Nickname = agent.Nickname,
                Status = StatusAgentEnum.Dormant
            };
            context.Agents.Add(model);
            
            await context.SaveChangesAsync();
            if (model.Id == 0)
            {
                throw new Exception("bad request");
            }
            return new() { Id = model.Id };
        }

        public async Task<List<Agent>> GetAllAgentsAsync()
        {
            var allAgents = await context.Agents.ToListAsync();
            return allAgents;
        }
        public async Task<List<Agent>> GetAllAgentsWithMissionsAsync()
        {
            var allAgents = await context.Agents.AsNoTracking()
            .ToListAsync();
            
            foreach (var agent in allAgents)
            {
                var missions = await context.Missions
                    .Where(m => m.AgentId == agent.Id)
                    .ToListAsync();
                agent.Missions = missions;
            }
            return allAgents;
        }

        public async Task PinPositionAsync(int id, PositionDto position)
        {
            if (!MoveUtils.IsPositionLegal(position.X, position.Y))
            {
                throw new Exception("the Position is not legal");
            }
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null)
            {
                throw new Exception("agent is not found");
            }
            agent.XPosition = position.X;
            agent.YPosition = position.Y;
            await context.SaveChangesAsync();
        }

        public async Task MoveAgentAsync(int id, DirectionDto direction)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null)
            {
                throw new Exception("agent is not found");
            }
            if (agent.Status == StatusAgentEnum.Active)
            {
                throw new Exception("agent is active");
            }
            var moveTo = MoveUtils.Directions[direction.Direction];
            agent.XPosition += moveTo.x;
            agent.YPosition += moveTo.y;
            if (!MoveUtils.IsPositionLegal(agent.XPosition, agent.YPosition))
            {
                throw new Exception("the move is not legal");
            }
            await context.SaveChangesAsync();
            await _missionService.DeleteIrrelevantMissions();
            await _missionService.CreateMission(agent);
        }
    }
}