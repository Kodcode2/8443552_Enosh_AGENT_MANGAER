using AgentsRest.Data;
using AgentsRest.Models;
using AgentsRest.Enums;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using AgentsRest.Utils;
using AgentsRest.Dto;
//לטיפול ביום א. בידיקה את הניקוי שלשימות שלא רלוונטיות. ..==========================
//לבדוק מחיקה שמקרה והמשימה פעילה
//להוסיף הפעלת משימה
namespace AgentsRest.Service
{
    public class MissionService(
        ApplicationDbContext context/*,
        IServiceProvider serviceProvider*/
    ) : IMissionService
    {
        public async Task<List<Mission>> GetAllMissionsWithAgentAndTargetAsync() =>
            await context.Missions
            .Include(m => m.Agent)
            .Include(m => m.Target)
            .ToListAsync();

        public async Task<List<MissionDto>> GetAllMissionsDtoAsync()
        {
            var allMissions = await GetAllMissionsWithAgentAndTargetAsync();
            return allMissions
                .Where(m => m.Status == StatusMissionEnum.Proposal)
                .Select(m => new MissionDto()
                {
                    Id = m.Id,
                    AgentNickname = m.Agent.Nickname,
                    AgentXPosition = m.Agent.XPosition,
                    AgentYPosition = m.Agent.YPosition,
                    TargettName = m.Target.Name,
                    TargetRole = m.Target.Role,
                    TargetXPosition = m.Target.XPosition,
                    TargetYPosition = m.Target.YPosition,
                    Distance = MoveUtils.Distance(m.Agent, m.Target),
                    Duration = TimeSpan.FromHours(MoveUtils.Distance(m.Agent, m.Target) / 5)
                })
                .ToList();
        }
        private async Task<Mission> GetMissionWithAgentAndTargetByIdAsync(int id)
        {
            var mission = await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null)
            {
                throw new Exception("The mission does not exist");
            }
            return mission;
        }

        public async Task CreateMission(Agent agent)
        {
            if (agent.Status == StatusAgentEnum.Active)
            {
                throw new Exception("agent is active");
            }
            if (!MoveUtils.IsPositionLegal(agent.XPosition, agent.YPosition))
            {
                throw new Exception("agent is not in service");
            }
            var missions = await GetAllMissionsWithAgentAndTargetAsync();
            missions.Where(m => m.Status != StatusMissionEnum.Finished)
                .ToList();
            var targets = await /*targetService.GetAllTargetsAsync(); */ context.Targets.ToListAsync();
            targets = targets
                //הבאת רק טרוריסטים חיים
                .Where(target => target.Status == StatusTargetEnum.Lives)
                //הבאת רק טרוריסטים שמיקומם אותחל
                .Where(target => MoveUtils.IsPositionLegal(target.XPosition, target.YPosition))
                //הבאת רק טרוריסטים שמיקומם הוא בתוך 200 ק"מ
                .Where(target => MoveUtils.Distance(agent, target) <= 200)
                //הבאת רק טרוריסטים שלא נמצאים במשימה שהופעלה כבר
                .Where(target => !missions.Any(m => m.TargetId == target.Id && m.Status == StatusMissionEnum.Assigned))
                //הבאת רק טרוריסטים שלא נמצאים במשימה קיימת עם סוכן זה
                .Where(target => !missions.Any(m => m.TargetId == target.Id && m.AgentId == agent.Id))
                .ToList();
            await context.Missions.AddRangeAsync(targets
                //הבאת רק טרוריסטים שלא נמצאים במשימה שהופעלה כבר
                .Where(target => !missions.Any(m => m.TargetId == target.Id && m.Status == StatusMissionEnum.Assigned))
                //הבאת רק טרוריסטים שלא נמצאים במשימה קיימת עם סוכן זה
                .Where(target => !missions.Any(m => m.TargetId == target.Id && m.AgentId == agent.Id))
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
            var missions = await GetAllMissionsWithAgentAndTargetAsync();
            missions.Where(m => m.Status != StatusMissionEnum.Finished)
                .ToList();

            if (missions.Any(m => m.Target == target))
            {
                throw new Exception("The target is already being tracked");
            }
            var agents = await /*agentsService.GetAllAgentsAsync(); */ context.Agents.ToListAsync();
            agents = agents
                //הבאת כל הסוכנים שהופעלו
                .Where(agent => MoveUtils.IsPositionLegal(agent.XPosition, agent.YPosition))
                //הבאת כל הסוכנים שאינם פעילים
                .Where(agent => agent.Status == StatusAgentEnum.Dormant)
                //הבאת כל הסוכנים שנמצאים בתוך 200 ק"מ
                .Where(agent => MoveUtils.Distance(agent, target) <= 200)
                //הבאת כל הסוכנים שאין עליהם משימה עם הטרוריסט הספציפי
                .Where(agent => !missions.Any(m => m.TargetId == target.Id && m.AgentId == agent.Id))
                .ToList();

            await context.Missions.AddRangeAsync(agents
                //הבאת כל הסוכנים שאין עליהם משימה עם הטרוריסט הספציפי
                .Where(agent => !missions.Any(m => m.TargetId == target.Id && m.AgentId == agent.Id))
                //הבאת כל הסוכנים שאינם פעילים
                .Where(agent => agent.Status == StatusAgentEnum.Dormant)
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
            var missionsProposal = await GetAllMissionsWithAgentAndTargetAsync();
            missionsProposal = missionsProposal.Where(m => m.Status == StatusMissionEnum.Proposal)
                .ToList();

            context.Missions.RemoveRange(missionsProposal
                .Where(m => MoveUtils.Distance(m.Agent, m.Target) > 200)
            );
            var toRemove = missionsProposal
                 .FirstOrDefault(
                    mission => missionsProposal.Any(m => m.TargetId == mission.TargetId && m.AgentId == mission.AgentId)
                 );
            if (toRemove != null)
            {
                context.Missions.Remove(toRemove);
            }
            await context.SaveChangesAsync();
            ;
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

        public async Task MoveAllActiveAgentsTowardsTargetAsync()
        {
            var mission = await GetAllMissionsWithAgentAndTargetAsync();
            mission = mission.Where(mission => mission.Status == StatusMissionEnum.Assigned)
                .ToList();

            mission.ForEach(async m => await MoveAgentTowardsTarget(m));
            await context.SaveChangesAsync();
        }

        public async Task MoveAgentTowardsTarget(Mission mission)
        {
            var direction = MoveUtils.GetDirectionToActiveAgent(mission);
            
            if (direction != "")
            {
                var position = MoveUtils.Directions[direction];
                mission.Agent.XPosition += position.x;
                mission.Agent.YPosition += position.y;
                mission.TimeLeft = MoveUtils.Distance(mission.Agent, mission.Target);
            }
            if (direction == "" || MoveUtils.GetDirectionToActiveAgent(mission) == "")
            {
                await CompleteMission(mission);
            }
        }
        public async Task RunMission(int id)
        {
            var mission = await GetMissionWithAgentAndTargetByIdAsync(id)
                ?? throw new Exception("The mission does not exist");
            if(mission.Agent.Status == StatusAgentEnum.Active)
            {
                throw new Exception("The mission has already been started");
            }
            mission.Agent.Status = StatusAgentEnum.Active;
            mission.Status = StatusMissionEnum.Assigned;
            var allMissions = await GetAllMissionsWithAgentAndTargetAsync();
            context.Missions.RemoveRange(allMissions
                .Where(m => m.Id != id)
                .Where(m => m.TargetId == mission.TargetId || m.Agent.Id == mission.AgentId)
                .ToList()
            );
            await context.SaveChangesAsync();
        }
    }
}
