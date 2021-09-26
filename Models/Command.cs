using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Command
    {
        public int Id { get; set; }

        [Required] //Used to specify Not Nullable for Entity Framework
        [MaxLength(250)]//Used to specifcy Max Length for Entity Framework

        public string HowTo { get; set; }
        [Required] //Used to specify Not Nullable for Entity Framework

        public string Line { get; set; }
        [Required] //Used to specify Not Nullable for Entity Framework

        public string Platform { get; set; }
    }
}