using System.ComponentModel.DataAnnotations;

namespace Books.Api.Models
{
    public class BookForCreation
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
    }
}
