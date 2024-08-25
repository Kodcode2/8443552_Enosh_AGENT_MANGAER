using AgentsClient.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentsClient.Controllers
{
    public class MissionController(IMissionService missionService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var missions = await missionService.GetAllMissionsAsync();
            return View(missions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mission = await missionService.GetMissionAsync(id);
            return View(mission);
        }

        public async Task<IActionResult> Run(int id)
        {
            try
            {
                var mission = await missionService.RunMissionAsync(id);

                return View("Details", mission);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
