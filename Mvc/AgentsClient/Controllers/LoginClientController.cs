using AgentsClient.Dto;
using AgentsClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using AgentsClient.Service;

namespace AgentsClient.Controllers
{
    public class LoginClientController(ILoginService loginService) : Controller
    {
        private readonly string loginApi = "https://localhost:7083/Login";

        public IActionResult Index() => View();
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            try
            {
                await loginService.LoginAsync();
                return RedirectToAction("index", "Table");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
    }
}