using AgentsRest.Enums;

namespace AgentsRest.Utils
{
    public static class Move
    {
        public static Dictionary<string, (int x, int y)> DirectionsDictionary = new()
        {
            {"se", (1, -1) },
            {"e", (1, 0) },
            {"en", (1, 1) },
            {"n", (0, 1) },
            {"nw", (-1, 1) },
            {"w", (-1, 0) },
            {"ws", (-1, -1) },
            {"s", (0, -1) }
        };
        public static bool IsPositionLegal(int x, int y) =>
            x >= 0 && y >= 0 && x <= 1000 && y <= 1000;
        /*
        public static (int x, int y) CalculateMove(DirectionsEnum direction) => direction switch
        {
            DirectionsEnum.SouthEast => (1, -1),
            DirectionsEnum.East => (1, 0),
            DirectionsEnum.EastNorth => (1, 1),
            DirectionsEnum.North => (0, 1),
            DirectionsEnum.NorthWest => (-1, 1),
            DirectionsEnum.West => (-1, 0),
            DirectionsEnum.WestSouth => (-1, -1),
            DirectionsEnum.South => (0, -1),
            _ => (0, 0)
        };*/
    }
}