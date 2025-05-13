namespace kolos1.Models;

public class DeliveryAddDTO
{
    public int deliveryId { get; set; }
    public int customerId { get; set; }
    public string licenceNumber { get; set; }
    public List<ProductAddDTO> products { get; set; }
}