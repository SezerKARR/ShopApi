namespace Shop.Application.Services;

using AutoMapper;
using Domain.Models;
using Domain.Models.Common;
using Dtos.OrderItem;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

public interface IOrderItemService {
    Task<Response<ReadOrderItemDto>> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto,int orderId);
}

public class OrderItemService:IOrderItemService {
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderItem> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public OrderItemService(IOrderItemRepository orderItemRepository, ILogger<OrderItem> logger, IMapper mapper, IUnitOfWork unitOfWork) {
        _orderItemRepository = orderItemRepository;
        this._logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response<ReadOrderItemDto>> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto,int orderId) {
        var productSeller=await _unitOfWork.ProductSellerRepository.GetByIdAsync(createOrderItemDto.ProductSellerId);
        if(productSeller == null)
            return new Response<ReadOrderItemDto>("Product Seller Not Found");
        try
        {
            var orderItem=_mapper.Map<OrderItem>(createOrderItemDto);
            orderItem.OrderId = orderId;
            await _orderItemRepository.CreateAsync(orderItem);
            if (await _unitOfWork.CommitAsync())
            {
                return new Response<ReadOrderItemDto>("An error occurred when creating a new order item.");
                
            }
            ReadOrderItemDto readOrderItemDto = _mapper.Map<ReadOrderItemDto>(orderItem);
            return new Response<ReadOrderItemDto>(readOrderItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ReadOrderItemDto>(e.Message);
        }
    }
}
