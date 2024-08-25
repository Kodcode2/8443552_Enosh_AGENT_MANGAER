using AgentsClient.Enums;
using AgentsClient.Models;

namespace AgentsClient.ViewModel
{
    public class AgentVM
    {
        public int Id { get; set; }
        public required string Image { get; set; }
        public required string Nickname { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public StatusAgentEnum Status { get; set; }
        public TimeSpan Duration { get; set; }
        public int AmountamountEliminations {  get; set; }
        public List<Mission> Missions { get; set; } = [];
    }
}
