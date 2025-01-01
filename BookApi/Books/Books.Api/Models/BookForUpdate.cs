using System.ComponentModel.DataAnnotations;

namespace Books.Api.Models
{
    public class BookForUpdate
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
    }
}
