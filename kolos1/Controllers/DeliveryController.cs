using APBD_example_test1_2025.Exceptions;
using apdb25_pk1.Services;
using kolos1.Models;
using kolos1.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolos1.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _service;

    public DeliveryController(IDeliveryService service)
    {
        _service = service;
    }
    
    [HttpGet("{id}/deliveries")]
    public async Task<IActionResult> GetDeliveryAsync(int id, CancellationToken cancellationToken)
    {
        var delivery = await _service.GetDeliveryAsync(id, cancellationToken);
        if (delivery == null)
        {
            return NotFound($"Delivery with ID {id} not found.");
        }

        return Ok(delivery);
    }
    [HttpPost("{id}/deliveries")]
    public async Task<IActionResult> AddDeliveryAsync(DeliveryAddDTO delivery, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.AddDeliveryAsync(delivery, cancellationToken);
            return result switch
            {
                ICustomerService.AddRentalResult.NotFound => NotFound("Customer or one of the movies does not exist."),
                ICustomerService.AddRentalResult.Success => Ok("Rental created successfully."),
                ICustomerService.AddRentalResult.Error => StatusCode(500, "An error occurred during the operation."),
                _ => BadRequest("Invalid request.")
            };

        }catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}