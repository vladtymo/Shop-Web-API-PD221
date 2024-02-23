namespace Core.DTOs
{
    public class OrderSummaryModel
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
