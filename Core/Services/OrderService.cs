using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Services.Specifications;
using Services_Abstractions;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(
        IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            var address = mapper.Map<Address>(orderRequest.ShipToAddress);

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if ( basket is null ) throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product , int>().GetAsync(item.Id);
                if( product is null ) throw new ProductNotFoundException(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity , (int)product.Price);
                orderItems.Add(orderItem);  
            }

            var deliveryMethod = await unitOfWork .GetRepository<DeliveryMethod,int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var subTotal = orderItems.Sum( i => i.Quantity * i.Price );

            var order = new Order(userEmail, address,orderItems,deliveryMethod, subTotal,"");
            await unitOfWork.GetRepository<Order,Guid>().AddAsync(order);
            var count = await unitOfWork.SaveChangesAync();
            if (count == 0) throw new OrderCreateBadRequestException();

            var result = mapper.Map<OrderResultDto>(order);  

            return result;
        }

        public async Task<IEnumerable<DeliveryMothedDto>> GetDeliveryMotheds()
        {
            var deliverMotheds = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMothedDto>>(deliverMotheds);

            return result;
        }

        public async Task<OrderResultDto> GetOrderByIDAsync(Guid id)
        {
            var spec = new  OrderSpecifications(id);
            var order = await  unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if (order is null) throw new OrderNotFoundException(id);
           var result = mapper.Map<OrderResultDto>(order);

            return result;
        }

        public async Task<IEnumerable<OrderResultDto>> GetOrderByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
           
            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);

            return result;
        }
    }
}
