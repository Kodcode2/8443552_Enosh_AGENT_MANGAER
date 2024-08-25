using Humanizer;

namespace AgentsClient.ViewModel
{
    public class GeneralInformationVM
    {
        public int TotalAgents { get; set; }
        public int AllActiveAgents { get; set; }
        public int TotalTargets { get; set; }
        public int AllEliminatedTargets { get; set; }
        public int TotalMissions { get; set; }
        public int AllAssignedMissions { get; set; }
        public double RelationOfAgentsToTargets { get; set; }
        public double RatioOfAgentsThatCanBeTeamedToTargetsAgainstTargets { get; set; }
    }
}
