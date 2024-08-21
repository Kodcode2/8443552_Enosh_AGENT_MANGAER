using AgentsRest.Enums;

namespace AgentsRest.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public double TimeLeft { get; set; }
        public DateTime ActualExecutionTime { get; set; }
        public StatusMissionEnum Status { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
    }
}
