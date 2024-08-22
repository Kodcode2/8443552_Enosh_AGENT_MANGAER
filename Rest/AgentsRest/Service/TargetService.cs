using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Enums;
using AgentsRest.Models;
using AgentsRest.Utils;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {

        public async Task<IdDto> CreateTargetAsync(TargetDto target)
        {
            Target model = new()
            {
                Name = target.name,
                Position = target.position,
                Status = StatusTargetEnum.Lives
            };
            context.Targets.Add(model);

            await context.SaveChangesAsync();
            if (model.Id == 0)
            {
                throw new Exception("bad request");
            }
            return new() { Id = model.Id };
        }

        public async Task<List<Target>> GetAllTargetsAsync()
        {
            var allTargets = await context.Targets.ToListAsync();
            return allTargets;
        }

        public async Task MoveTargetAsync(int id, DirectionDto direction)
        {
            var target = await context.Targets.FirstOrDefaultAsync(a => a.Id == id);
            if (target == null)
            {
                throw new Exception("target is not found");
            }
            if (target.Status == StatusTargetEnum.Eliminated)
            {
                throw new Exception("target is eliminated");
            }
            var moveTo = Move.DirectionsDictionary[direction.Direction];
            target.XPosition += moveTo.x;
            target.YPosition += moveTo.y;
            if (!Move.IsPositionLegal(target.XPosition, target.YPosition))
            {
                throw new Exception("the move is not legal");
            }
            await context.SaveChangesAsync();
        }

        public async Task PinPositionAsync(int id, PositionDto position)
        {
            if (!Move.IsPositionLegal(position.X, position.Y))
            {
                throw new Exception("the Position is not legal");
            }
            var target = await context.Targets.FirstOrDefaultAsync(a => a.Id == id);
            if (target == null)
            {
                throw new Exception("target is not found");
            }
            if (target.Status == StatusTargetEnum.Eliminated)
            {
                throw new Exception("target is eliminated");
            }
            target.XPosition = position.X;
            target.YPosition = position.Y;
            await context.SaveChangesAsync();
        }
    }
}
