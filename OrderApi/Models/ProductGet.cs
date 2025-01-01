namespace OrderApi.Models
{
    public class ProductGet
    {
        public string ProductId { get; set; }
        public ProdDto Product { get; set; }
    }

    public class ProdDto
    {
        public string ProductName { get; set; }
    }
}
