using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class Point
    {
        [Key]
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point() { X = -1; Y = -1; }
    }
}
