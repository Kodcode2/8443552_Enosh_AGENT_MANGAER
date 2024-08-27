using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Enums;
using AgentsRest.Models;
using AgentsRest.Utils;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Service
{
    public class AgentsService(
        ApplicationDbContext context, 
        IServiceProvider serviceProvider
    ) : IAgentsService
    {
        private IMissionService _missionService => serviceProvider.GetRequiredService<IMissionService>();
        //יצירת סוכן
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
                throw new Exception("No agent created");
            }
            return new() { Id = model.Id };
        }

        //קבלת כל הסוכנים
        public async Task<List<Agent>> GetAllAgentsAsync() =>
            await context.Agents.ToListAsync();

        //קבלת סוכן ספציפי לפי ID
        public async Task<Agent> GetAgentByIdAsync(int id)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null)
            {
                throw new Exception("agent is not found");
            }
            return agent;
        }

        //קבלת כל הסוכנים והמשימות המקושרות להם
        public async Task<List<Agent>> GetAllAgentsWithMissionsAsync()
        {
            var allAgents = await context.Agents.AsNoTracking().ToListAsync();

            var allMissions = await context.Missions.ToListAsync();

            return (from agent in allAgents
                     join mission in allMissions on agent.Id equals mission.AgentId
                     select new Agent () 
                     {
                         Id = agent.Id,
                         Image = agent.Image,
                         Nickname = agent.Nickname,
                         Status = agent.Status,
                         XPosition = agent.XPosition,
                         YPosition = agent.YPosition,
                         Missions = [..agent.Missions, mission]
                     }).ToList();
        }

        //הצבת סוכן במיקום ראשוני
        public async Task PinAgentAsync(int id, PositionDto position)
        {
            if (!MoveUtils.IsPositionLegal(position.X, position.Y))
            {
                throw new Exception("the Position is not legal");
            }
            var agent = await GetAgentByIdAsync(id);
            agent.XPosition = position.X;
            agent.YPosition = position.Y;
            await context.SaveChangesAsync();
        }

        //הזזת סוכן
        public async Task MoveAgentAsync(int id, DirectionDto direction)
        {
            var agent = await GetAgentByIdAsync(id);

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
            //מחיקת כל המשימות שכבר בוטלו
            await _missionService.DeleteIrrelevantMissions();
            //יצירת המשימות החדשות
            await _missionService.CreateMission(agent);
        }
    }
}