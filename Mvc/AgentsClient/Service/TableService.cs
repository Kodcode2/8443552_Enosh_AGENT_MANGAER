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
            PositionsVM positions = new PositionsVM();
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                {
                    if (targets.Any(t => t == (i, j)))
                    {
                        positions.Matrix[i, j] = "[#]";
                    }
                    else if (agents.Any(a => a == (i, j)))
                    {
                        positions.Matrix[i, j] = "[$]";
                    }
                    else
                    {
                        positions.Matrix[i, j] = "[X]";
                    }
                }
            return positions;
        }
    }
}
