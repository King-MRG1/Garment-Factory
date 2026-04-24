namespace Shared.Dtos.OrderDtos
{
    public class ViewOrderModelDto
    {
        public int Model_Id { get; set; }
        public int Order_Id { get; set; }
        public string Model_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
