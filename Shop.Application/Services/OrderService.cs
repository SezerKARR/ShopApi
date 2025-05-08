namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.Order;
using Helpers;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

public interface IOrderService {
    Task<Response<Order>> CreateOrderAsync(CreateOrderDto createOrder);
}

public class OrderService:IOrderService {
    private readonly IOrderItemService _orderItemService;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<Order> _logger;
    private readonly IMapper _mapper;
    private readonly ITransactionScopeService _transactionScopeService;
    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ILogger<Order> logger, IMapper mapper, IOrderItemService orderItemService, ITransactionScopeService transactionScopeService) {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _orderItemService = orderItemService;
        _transactionScopeService = transactionScopeService;
    }
    public async Task<Response<Order>> CreateOrderAsync(CreateOrderDto createOrder) {
        return await _transactionScopeService.ExecuteAsync(async () => {

            var user = await _unitOfWork.UserRepository.GetByIdAsync(createOrder.UserId);
            if (user == null)
                return new Response<Order>("User not found");
            try
            {
                var order = _mapper.Map<Order>(createOrder);
                await _orderRepository.CreateAsync(order);
                if (!await _unitOfWork.CommitAsync())
                {
                    return new Response<Order>("An error occurred when creating a new order.");
                }
                foreach (var orderItem in createOrder.OrderItems)
                {
                    var response = await _orderItemService.CreateOrderItemAsync(orderItem,order.Id);
                    if (!response.Success)
                    {
                        if (response.Message != null)
                            return new Response<Order>(response.Message);
                    }

                }
                return new Response<Order>(order);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Response<Order>(e.Message);
            }


        });
    }
}