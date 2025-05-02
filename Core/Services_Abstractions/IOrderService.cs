using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstractions
{
    public interface IOrderService
    {
        Task<OrderResultDto> GetOrderByIDAsync(Guid id);
        Task<IEnumerable<OrderResultDto>> GetOrderByUserEmailAsync(string userEmail);
        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest , string userEmail);
        Task<IEnumerable<DeliveryMothedDto>> GetDeliveryMotheds();

    }
}
