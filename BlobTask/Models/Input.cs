using System.ComponentModel.DataAnnotations;

namespace BlobTask.Models
{
    public class Input
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]

        public string EmailName { get; set; }
        [Required]
        [ValidateDocx(".docx")]
        public IFormFile File { get; set; }
    }
}
