using apdb25_pk1.Models;
using kolos1.Models;
using Microsoft.Data.SqlClient;

namespace kolos1.Services;

public class DeliveryService : IDeliveryService
{
    
    private readonly string _connectionString;
    public DeliveryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }
    
    private IDeliveryService _deliveryServiceImplementation;
    
    public async Task<DeliveryDTO> GetDeliveryAsync(int delivery_id, CancellationToken cancellationToken)
    {
        DeliveryDTO? delivary = null;
        var driversDict = new Dictionary<int, DriverDTO>();
        var customersDict = new Dictionary<int, Customer2DTO>();
        var productsDict = new Dictionary<int, ProductDTO>();
        
        string command = @"SELECT 
            d.date,
            c.first_name, 
            c.last_name, 
            c.date_of_birth, 
            dr.first_name,
            dr.last_name,
            dr.licence_number,
            p.name,
            p.price
            FROM Delivery d   
            LEFT JOIN Customer c ON c.customer_id = d.customer_id                        
            LEFT JOIN Driver dr ON dr.driver_id = d.driver_id
            LEFT JOIN Product_Delivery dp ON dp.delivery_id = d.delivery_id
            LEFT JOIN Product p ON p.product_id = dp.product_id
            WHERE d.delivery_id = @id";
        
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(command, conn);
        cmd.Parameters.AddWithValue("@id", delivery_id);
        await conn.OpenAsync(cancellationToken);
        
        using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            if (delivary == null)
            {
                delivary = new DeliveryDTO
                {
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    customer = new List<Customer2DTO>(),
                    driver = new List<DriverDTO>(),
                    products = new List<ProductDTO>()
                };
            }

            if (!reader.IsDBNull(reader.GetOrdinal("customer_id")))
            {
                var customerId = reader.GetInt32(reader.GetOrdinal("customer_id"));
                if (!customersDict.ContainsKey(customerId))
                {
                    var customer = new Customer2DTO()
                    {
                        firstName = reader.GetString(reader.GetOrdinal("first_name")),
                        lastName = reader.GetString(reader.GetOrdinal("last_name")),
                        dateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"))
                    };
                    customersDict[customerId] = customer;
                    delivary.customer.Add(customer);
                }
            }
            if (!reader.IsDBNull(reader.GetOrdinal("driver_id")))
            {
                var driverId = reader.GetInt32(reader.GetOrdinal("driver_id"));
                if (!driversDict.ContainsKey(driverId))
                {
                    var driver = new DriverDTO()
                    {
                        firstName = reader.GetString(reader.GetOrdinal("first_name")),
                        lastName = reader.GetString(reader.GetOrdinal("last_name")),
                        licenceNumber = reader.GetString(reader.GetOrdinal("licence_number"))
                    };
                    driversDict[driverId] = driver;
                    delivary.driver.Add(driver);
                }
            }
            if (!reader.IsDBNull(reader.GetOrdinal("product_id")))
            {
                var productId = reader.GetInt32(reader.GetOrdinal("product_id"));
                if (!productsDict.ContainsKey(productId))
                {
                    var product = new ProductDTO()
                    {
                        name = reader.GetString(reader.GetOrdinal("name")),
                        price = reader.GetDecimal(reader.GetOrdinal("price")),
                        amount = 0
                    };
                    productsDict[productId] = product;
                    delivary.products.Add(product);
                }
            }
        }
        return delivary;
    }

    public async Task<IDeliveryService.AddDeliveryResult> AddDeliveryAsync(DeliveryAddDTO delivery, CancellationToken cancellationToken)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        using var tran = conn.BeginTransaction();

        try
        {
            // Sprawdź wyjątki
            
            
            // Dodaj delivery
            var insertProductCmd = new SqlCommand( 
                "INSERT INTO Product_Delivery (product_id, Delivery_id) VALUES (@product_id, @delivery_id)", 
                conn, tran);
            
            var insertDeliveryCmd = new SqlCommand(
                "INSERT INTO Delivery (delivery_id, customer_id, driver_id, date) VALUES (@id, @customer_id, @driver_id, CURRENT_DATE)",
                conn, tran);
            
            insertDeliveryCmd.Parameters.AddWithValue("@id", );
            insertDeliveryCmd.Parameters.AddWithValue("@rentalDate", rentalPutDTO.rentalDate);
            insertDeliveryCmd.Parameters.AddWithValue("@customerId", customer_id);
            insertDeliveryCmd.Parameters.AddWithValue("@statusId", statusId);
            await insertDeliveryCmd.ExecuteNonQueryAsync(cancellationToken);
            
            insertProductCmd.Parameters.AddWithValue("@id", rentalPutDTO.id);
            insertProductCmd.Parameters.AddWithValue("@rentalDate", rentalPutDTO.rentalDate);
            insertProductCmd.Parameters.AddWithValue("@customerId", customer_id);
            insertProductCmd.Parameters.AddWithValue("@statusId", statusId);
            await insertProductCmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}