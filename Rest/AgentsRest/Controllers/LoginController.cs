using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController(
        IJwtService jwtService
    ) : ControllerBase
    {
        //השמות המאושרים לכניסה
        private static readonly ImmutableList<string> _allowedIds = [
            "SimulationServer", "MVCServer"
        ];

        [HttpPost]
        public ActionResult Login([FromBody] LoginDto login) =>
            _allowedIds.Contains(login.Id)
                ? Ok(new TokenDto() { Token = jwtService.CreateToken(login.Id) })
                : BadRequest();
    }
            
}
