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
        public int XPosition { get; set; } = -1;
        public int YPosition { get; set; } = -1;
        public StatusAgentEnum Status {  get; set; }
    }
}
