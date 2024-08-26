using AgentsClient.Enums;
using AgentsClient.Models;
using AgentsClient.Service;
using AgentsClient.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentsClient.Controllers
{
    public class DashboardsController(
        IMissionService missionService,
        IAgentService agentService,
        ITargetService targetService
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            Statistics informationVM = await missionService.GetStatisticsAsync();
            return View(informationVM);
        }
        public async Task<IActionResult> Agents()
        {
            var agents = await agentService.GetAllAgentsVMAsync();
            return View(agents);
        }
        public async Task<IActionResult> Mission( int agentId)
        {
            var agent = await agentService.GetAgentsVMByIdAsync(agentId);
            if (agent.Status == StatusAgentEnum.Active)
            {
                return View(agent.Missions.FirstOrDefault());
            }
            return RedirectToAction("Agents");
        }

        public async Task<IActionResult> Targets()
        {
            var targets = await targetService.GetAllTargetsAsync();
            return View(targets);
        }
    }
}
