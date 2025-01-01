using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RatingApi.Entities;

public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    [Range(1, 5)]
    public int RatingValue { get; set; }

}