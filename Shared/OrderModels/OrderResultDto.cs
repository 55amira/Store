using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class OrderResultDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }

        // ShippingAddress
        public AddressDto ShippingAddress { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        // Delivery Method
        public string DeliveryMethod { get; set; }
      

        // Payment Status
        public string PaymentStatus { get; set; } 

        //Sun total
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        // Order Data
        public DateTimeOffset OrderData { get; set; } = DateTimeOffset.Now;

        // Payment
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
