using Shared.Enums.Order;

namespace Shared.Dtos.Order;
public class OrderDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippingAddress { get; set; }
    public string InvoiceAddress { get; set; }
    public EOrderStatus Status { get; set; }
}
