using AgentsClient.Enums;
using AgentsClient.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentsClient.Controllers
{
    public class MissionController(IMissionService missionService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var missions = await missionService.GetAllMissionsAsync();
            var missionsToView = missions.Where(m => m.status == StatusMissionEnum.Proposal);
            return View(missionsToView);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mission = await missionService.GetMissionByIdAsync(id);
            return View(mission);
        }

        public async Task<IActionResult> Run(int id)
        {

            var isRun = await missionService.RunMissionAsync(id);
            if (isRun)
            {
                return RedirectToAction("Details", new { id });
            }
            return RedirectToAction("Index");
        }
    }
}