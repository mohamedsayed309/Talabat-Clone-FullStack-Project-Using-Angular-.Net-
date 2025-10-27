using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(IBasketRepository basketRepo,IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            ///_productRepo = productRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_ordersRepo = ordersRepo;
        }
        public async Task<Order> CreatOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1. Get Basket From Basket Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);


            //2. Get Selected items at basket from product repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                var productRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetAsync(item.Id);

                    var ProductItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);

                    var orderItem = new OrderItem(ProductItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);


                }
            }

            //3. Calculate SubTotal
            var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            //4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            //5. Create Order
            var order = new Order(buyerEmail,shippingAddress, deliveryMethod, orderItems, subtotal);

            await _unitOfWork.Repository<Order>().AddAsync(order);

            //6.Save To Database
           var result =  await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo =  _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(orderId, buyerEmail);

            var order = await orderRepo.GetWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        
    }
}
