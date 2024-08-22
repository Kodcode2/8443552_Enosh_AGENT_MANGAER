using AgentsRest.Data;
using AgentsRest.Models;
using AgentsRest.Enums;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using AgentsRest.Utils;

namespace AgentsRest.Service
{
    public class MissionService(
        ApplicationDbContext context/*,
        IServiceProvider serviceProvider*/
    ) : IMissionService
    {
        /*private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();
        private IAgentsService agentsService = serviceProvider.GetRequiredService<IAgentsService>();*/
        public async Task CreateMission(Agent agent)
        {
            if (agent.Status == StatusAgentEnum.Active)
            {
                throw new Exception("agent is active");
            }
            if(!MoveUtils.IsPositionLegal(agent.XPosition, agent.YPosition))
            {
                throw new Exception("agent is not in service");
            }
            var missionsAssigned = await context.Missions
                .Include(m => m.Target)
                .Include(m => m.Agent)
                .Where(m => m.Status == StatusMissionEnum.Assigned)
                .ToListAsync();
            var targets = await /*targetService.GetAllTargetsAsync(); */ context.Targets.ToListAsync();

            await context.Missions.AddRangeAsync(targets
                .Where(target => target.Status == StatusTargetEnum.Lives)
                .Where(target => !missionsAssigned
                                    .Any(m => m.Target == target && m.Status == StatusMissionEnum.Assigned))
                .Where(target => !missionsAssigned.Any(m=>m.Target==target&&m.Agent == agent))
                .Where(target => MoveUtils.IsPositionLegal(target.XPosition, target.YPosition))
                .Where(target => MoveUtils.Distance(agent, target) <= 200)
                .Select(target => new Mission()
                {
                    TimeLeft = MoveUtils.Distance(agent, target) / 5,
                    Status = StatusMissionEnum.Proposal,
                    AgentId = agent.Id,
                    TargetId = target.Id,
                    Agent = agent,
                    Target = target
                })
            );
            await context.SaveChangesAsync();
        }

        public async Task CreateMission(Target target)
        {
            if (target.Status == StatusTargetEnum.Eliminated)
            {
                throw new Exception("target is eliminated");
            }
            if (!MoveUtils.IsPositionLegal(target.XPosition, target.YPosition))
            {
                throw new Exception("target is not in service");
            }
            var missionsAssigned = await context.Missions
                .Include(m=>m.Target)
                .Include(m=>m.Agent)
                .Where(m => m.Status == StatusMissionEnum.Assigned)
                .ToListAsync();

            if (missionsAssigned.Any(m => m.Target == target))
            {
                throw new Exception("The target is already being tracked");
            }
            var agents = await /*agentsService.GetAllAgentsAsync(); */ context.Agents.ToListAsync();

            await context.Missions.AddRangeAsync(agents
                .Where(agent => agent.Status == StatusAgentEnum.Dormant)
                .Where(agent=>MoveUtils.IsPositionLegal(agent.XPosition,agent.YPosition))
                .Where(agent => !missionsAssigned.Any(m => m.Target == target && m.Agent == agent))
                .Where(agent => MoveUtils.Distance(agent, target) <= 200)
                .Select(agent => new Mission()
                {
                    TimeLeft = MoveUtils.Distance(agent, target) / 5,
                    Status = StatusMissionEnum.Proposal,
                    AgentId = agent.Id,
                    TargetId = target.Id,
                    Agent = agent,
                    Target = target
                })
            );
            await context.SaveChangesAsync();
        }

        public async Task DeleteIrrelevantMissions()
        {
            var missionsProposal = await context.Missions
                .Include(m=>m.Agent)
                .Include(m=>m.Target)
                .Where(m => m.Status == StatusMissionEnum.Proposal)
                .ToListAsync();

            context.Missions.RemoveRange(missionsProposal
                .Where(m => MoveUtils.Distance(m.Agent, m.Target) > 200)
            );
            await context.SaveChangesAsync();
        }

        private async Task CompleteMission(Mission mission)
        {
            if (mission == null)
            {
                throw new Exception("not found mission");
            }

            mission.TimeLeft = 0;
            mission.Status = StatusMissionEnum.Finished;
            mission.Agent.Status = StatusAgentEnum.Dormant;
            mission.Target.Status = StatusTargetEnum.Eliminated;
            mission.ActualExecutionTime = DateTime.Now;
            await context.SaveChangesAsync();
        }

        public async Task MoveAllActiveAgentsTowardsTarget()
        {
            var mission = await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .Where(mission => mission.Status == StatusMissionEnum.Assigned)
                .ToListAsync();
            mission.Select(MoveAgentTowardsTarget);
            await context.SaveChangesAsync();
        }

        private async Task MoveAgentTowardsTarget(Mission mission)
        {
            var direction = MoveUtils.GetDirectionToActiveAgent(mission);
            if (direction == "")
            {
                await CompleteMission(mission);
            }
            else
            {
                var position = MoveUtils.Directions[direction];
                mission.Agent.XPosition += position.x;
                mission.Agent.YPosition += position.y;
                mission.TimeLeft = MoveUtils.Distance(mission.Agent, mission.Target) / 5;
            }
        }
    }
}
