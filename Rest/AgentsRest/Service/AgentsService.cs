using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Enums;
using AgentsRest.Models;
using AgentsRest.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Service
{
    public class AgentsService(ApplicationDbContext context) : IAgentsService
    {
        public async Task<IdDto> CreateAgentAsync(AgentDto agent)
        {
            Agent model = new()
            {
                Image = agent.photo_url,
                Nickname = agent.nickname,
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

        public async Task PinPositionAsync(int id, PositionDto position)
        {
            if (!Move.IsPositionLegal(position.X, position.Y))
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
            var moveTo = Move.DirectionsDictionary[direction.Direction];
            agent.XPosition += moveTo.x;
            agent.YPosition += moveTo.y;
            if (!Move.IsPositionLegal(agent.XPosition, agent.YPosition))
            {
                throw new Exception("the move is not legal");
            }
            await context.SaveChangesAsync();
        }
    }
}