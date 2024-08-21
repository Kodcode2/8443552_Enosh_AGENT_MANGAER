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
        public Point Position { get; set; } = new Point();
        public StatusTargetEnum Status { get; set; }
        public List<Mission> Missions { get; set; } = [];
    }
}
