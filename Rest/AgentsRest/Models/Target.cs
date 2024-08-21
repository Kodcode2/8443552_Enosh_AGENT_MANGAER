using AgentsRest.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(32,MinimumLength = 2)]
        public required string Name { get; set; }
        [Required, StringLength(128, MinimumLength = 4)]
        public required string Role { get; set; }
        public int XPosition { get; set; } = -1;
        public int YPosition { get; set; } = -1;
        public StatusTargetEnum Status { get; set; }
    }
}
