using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AgentsClient.Enums;

namespace AgentsClient.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public required string Image { get; set; }
        public required string Nickname { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public StatusAgentEnum Status { get; set; }
        public List<Mission> missions { get; set; } = [];
    }
}
