using AgentsClient.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentsClient.Controllers
{
    public class TableController(
        ITableService tableService
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var table = await tableService.GetAllPositionsAsync();
            return View(table);
        }
    }
}
