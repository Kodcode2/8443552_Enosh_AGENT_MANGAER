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
    public class LoginController(IJwtService jwtService) : ControllerBase
    {
        private static readonly ImmutableList<string> _allowedIds = [
            "SimulationServer", "ControlManager"
        ];

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto login)
        {
            var r = _allowedIds.Contains(login.Id);

            if (r) return Ok(new TokenDto() { Token = jwtService.CreateToken(login.Id) });
            return BadRequest();
        }
    }
            
}
