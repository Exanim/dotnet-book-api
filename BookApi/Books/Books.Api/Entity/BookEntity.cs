using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Api.Entity
{
    public class BookEntity
    {

        [Required]
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        public BookEntity(string title)
        {
            Title = title;
        }
    }
}
