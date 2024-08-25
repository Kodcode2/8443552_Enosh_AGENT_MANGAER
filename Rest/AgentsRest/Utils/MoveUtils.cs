using AgentsRest.Enums;
using AgentsRest.Models;
using Microsoft.AspNetCore.Routing;

namespace AgentsRest.Utils
{
    public static class MoveUtils
    {
        public static Dictionary<string, (int x, int y)> Directions = new()
        {
            {"se", (1, -1) },
            {"e", (1, 0) },
            {"ne", (1, 1) },
            {"n", (0, 1) },
            {"nw", (-1, 1) },
            {"w", (-1, 0) },
            {"sw", (-1, -1) },
            {"s", (0, -1) }
        };
        public static bool IsPositionLegal(int x, int y) =>
            x >= 0 && y >= 0 && x <= 1000 && y <= 1000;
        public static double Distance(Agent agent, Target target) =>
            Math.Sqrt(
                Math.Pow(agent.XPosition - target.XPosition, 2) +
                Math.Pow(agent.YPosition - target.YPosition, 2)
            );

        public static string GetDirectionToActiveAgent(Mission mission)
        {
            int x = mission.Agent.XPosition - mission.Target.XPosition;
            int y = mission.Agent.YPosition - mission.Target.YPosition;
            string eastWest = string.Empty;
            string northSouth = string.Empty;
            switch (x)
            {
                case > 0:
                    eastWest = "w";
                    break;
                case < 0:
                    eastWest = "e";
                    break;
                default:
                    eastWest = "";
                    break;
            }
            switch (y)
            {
                case > 0:
                    northSouth = "s";
                    break;
                case < 0:
                    northSouth = "n";
                    break;
                default:
                    northSouth = "";
                    break;
            }
            return northSouth + eastWest;
        }
    }
}
