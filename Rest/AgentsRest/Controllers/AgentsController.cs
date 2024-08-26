using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentsService agentsService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> CreateAgent([FromBody] AgentDto agent)
        {
            try
            {
                var agentId = await agentsService.CreateAgentAsync(agent);
                return CreatedAtAction(nameof(CreateAgent), agentId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllAgents()
        {
            try
            {
                var allAgents = await agentsService.GetAllAgentsAsync();
                return Ok(allAgents);
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
        [HttpGet("incloudMissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Agent>> GetAllAgentsWithMissions()
        {
            try
            {
                var allAgents = await agentsService.GetAllAgentsWithMissionsAsync();
                return Ok(allAgents);
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PinPosition(int id, [FromBody] PositionDto position)
        {
            try
            {
                await agentsService.PinPositionAsync(id, position);
                return Ok();
            }
            catch (Exception ex) { return NotFound(); }
        }
        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Move(int id, [FromBody] DirectionDto direction)
        {
            try
            {
                await agentsService.MoveAgentAsync(id, direction);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
