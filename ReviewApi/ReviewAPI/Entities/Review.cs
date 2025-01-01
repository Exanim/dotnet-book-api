using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewAPI.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [MaxLength(280)]
        public string ProductReview { get; set; } = string.Empty;

        //public Review(int reviewId, int userId, int productId, string productReview)
        //{
        //    ReviewId = reviewId;
        //    UserId = userId;
        //    ProductId = productId;
        //    ProductReview = productReview;
        //}
    }
}
