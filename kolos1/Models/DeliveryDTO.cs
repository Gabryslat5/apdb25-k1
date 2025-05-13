using apdb25_pk1.Models;

namespace kolos1.Models;

public class DeliveryDTO
{
    public DateTime date { get; set; }
    public List<Customer2DTO> customer { get; set; }
    public List<DriverDTO> driver { get; set; }
    public List<ProductDTO> products { get; set; }
}