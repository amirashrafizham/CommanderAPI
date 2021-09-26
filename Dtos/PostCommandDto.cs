using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class PostCommandDto
    {
        [Required]
        public string HowTo { get; set; }
        [Required]
        public string Line { get; set; }
        [Required] //If you don't specify this, whenever the client makes an incomplete POST request it will return 500: internal server error. This is something we do not want. This will tell the client it's their mistake, not a server error
        public string Platform { get; set; }
    }
}