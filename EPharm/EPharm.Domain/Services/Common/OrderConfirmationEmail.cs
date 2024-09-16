using System.Globalization;
using System.Text;
using EPharm.Domain.Interfaces.CommonContracts;
using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Services.Common;

public class OrderConfirmationEmail(IEmailService emailService) : IOrderConfirmationEmail
{
    public string GenerateEmail(Order order, AppIdentityUser customer)
    {
        var template = emailService.GetEmail("order-confirmation");
        if (template is null)
            throw new Exception("The email template for 'order-confirmation' was not found.");
        
        var orderItems = new StringBuilder();
        
        foreach (var item in order.OrderProducts)
        {
            orderItems.Append($@"
                <tr>
                    <td style=""padding: 10px; border-bottom: 1px solid #eee;"">{item.Product.Name}</td>
                    <td style=""padding: 10px; border-bottom: 1px solid #eee; text-align: right;"">{item.Quantity}</td>
                    <td style=""padding: 10px; border-bottom: 1px solid #eee; text-align: right;"">{item.Product.Price} ₼</td>
                </tr>");
        }

        var email = template
            .Replace("{CustomerName}", customer.FirstName + " " + customer.LastName)
            .Replace("{OrderNumber}", order.TrackingId)
            .Replace("{OrderDate}", order.CreatedAt.ToString("MMMM dd, yyyy"))
            .Replace("{OrderItems}", orderItems.ToString())
            .Replace("{TotalAmount}", order.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture) + "₼")
            .Replace("{ShippingAddress}", order.Address);

        return email;
    }
}