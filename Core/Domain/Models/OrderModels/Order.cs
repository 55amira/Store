using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModels
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; }

        // ShippingAddress
        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Delivery Method
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }

        // Payment Status
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

        //Sun total
        public decimal SubTotal { get; set; }

         // Order Data
        public DateTimeOffset OrderData { get; set; } = DateTimeOffset.Now;

        // Payment
        public string PaymentIntentId { get; set; }
    }
}
