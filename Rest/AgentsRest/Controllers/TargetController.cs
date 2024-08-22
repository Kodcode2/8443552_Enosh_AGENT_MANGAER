using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ITargetService targetService) : ControllerBase
    {
        [HttpPost("POST/targets")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAgent([FromBody] TargetDto target)
        {
            try
            {
                var targetId = await targetService.CreateTargetAsync(target);
                if (targetId == null)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                    //PreconditionFailed
                }
                return Created("", targetId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GET/targets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllAgents()
        {
            try
            {
                var allAgents = await targetService.GetAllTargetsAsync();
                return Ok(allAgents);
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
        [HttpPut("PUT/targets/{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PinPosition(int id, [FromBody] PositionDto position)
        {
            try
            {
                await targetService.PinPositionAsync(id, position);
                return Ok();
            }
            catch (Exception ex) { return NotFound(); }
        }
        [HttpPut("PUT/targets/{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Move(int id, [FromBody] DirectionDto direction)
        {
            try
            {
                await targetService.MoveTargetAsync(id, direction);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
