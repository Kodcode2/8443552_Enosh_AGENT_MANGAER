using AgentsRest.Enums;

namespace AgentsRest.Dto
{
    public class MissionDto
    {
        public int Id { get; set; }
        public  string? AgentNickname { get; set; }
        public int AgentXPosition {  get; set; }
        public int AgentYPosition { get; set; }
        public  string? TargettName { get; set; }
        public string? TargetRole { get; set; }
        public int TargetXPosition { get; set; }
        public int TargetYPosition { get; set; }
        public double Distance {  get; set; }
        public TimeSpan Duration { get; set; }
        public StatusMissionEnum Status { get; set; }
    }
}