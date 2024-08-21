using AgentsRest.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Image { get; set; }
        [Required, StringLength(32, MinimumLength = 2)]
        public required string Nickname { get; set; }
        public Point Position { get; set; } = new Point();
        public StatusAgentEnum Status {  get; set; }
        public List<Mission> Missions { get; set; } = [];
    }
}
