using AgentsClient.ViewModel;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace AgentsClient.Service
{
    public class TableService(
        IAgentService agentService,
        ITargetService targetService
    ) : ITableService
    {
        public async Task<PositionsVM> GetAllPositionsAsync()
        {
            var agents = await agentService.GetAllAgentPositions();
            var targets = await targetService.GetAllTargetPositions();
            int maxX = Math.Max(agents.Max(a => a.X), targets.Max(t => t.X)),
                MaxY = Math.Max(agents.Max(a => a.Y), targets.Max(t => t.Y));
            PositionsVM positions = new();
            positions.Matrix = new string[maxX, MaxY];
            for (int i = 0; i < maxX; i++)
                for (int j = 0; j < MaxY; j++)
                {
                    if (targets.Any(t => t == (i, j)))
                    {
                        positions.Matrix[i, j] = "T";
                    }
                    else if (agents.Any(a => a == (i, j)))
                    {
                        positions.Matrix[i, j] = "A";
                    }
                    else
                    {
                        positions.Matrix[i, j] = "*";
                    }
                }
            return positions;
        }
    }
}
