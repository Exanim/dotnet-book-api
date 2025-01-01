using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Entities
{
    
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int productId { get; set; }

        public int OrderId { get; set; } // Külső kulcs az OrderEntity-hez
        public OrderEntity Order { get; set; }
    }
}
