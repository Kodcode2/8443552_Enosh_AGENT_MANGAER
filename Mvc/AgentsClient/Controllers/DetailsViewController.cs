using AgentsClient.Enums;
using AgentsClient.Models;
using AgentsClient.Service;
using AgentsClient.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentsClient.Controllers
{
    public class DetailsViewController(
        IMissionService missionService,
        IAgentService agentService
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            GeneralInformationVM informationVM = await missionService.GetDetailsView();
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
    }
}
