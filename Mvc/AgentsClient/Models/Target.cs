using AgentsClient.Enums;

namespace AgentsClient.Models
{
    public class Target
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Image { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public StatusTargetEnum Status { get; set; }
    }
}
