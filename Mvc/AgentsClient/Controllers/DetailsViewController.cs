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
            var agents =await agentService.GetAllAgents();
            return View(agents);
        }
    }
}
