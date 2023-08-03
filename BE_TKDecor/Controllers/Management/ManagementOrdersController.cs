﻿using AutoMapper;
using BE_TKDecor.Core.Dtos.Notification;
using BE_TKDecor.Core.Dtos.Order;
using BE_TKDecor.Core.Response;
using BE_TKDecor.Hubs;
using BusinessObject;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Utility;

namespace BE_TKDecor.Controllers.Management
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.RoleAdmin)]
    public class ManagementOrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _order;
        private readonly IProductRepository _product;
        private readonly INotificationRepository _notification;
        private readonly IHubContext<NotificationHub> _hub;

        public ManagementOrdersController(IMapper mapper,
            IOrderRepository order,
            IProductRepository product,
            INotificationRepository notification,
            IHubContext<NotificationHub> hub)
        {
            _mapper = mapper;
            _order = order;
            _product = product;
            _notification = notification;
            _hub = hub;
        }

        // POST: api/ManagementOrders/GetOrder
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _order.GetAll();
            orders = orders.Where(x => !x.IsDelete)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            var result = _mapper.Map<List<OrderGetDto>>(orders);
            return Ok(new ApiResponse { Success = true, Data = result });
        }

        // GET: api/ManagementOrders/FindById/1
        [HttpGet("FindById/{id}")]
        public async Task<IActionResult> FindById(Guid id)
        {
            var order = await _order.FindById(id);
            if (order == null || order.IsDelete)
                return NotFound(new ApiResponse { Message = ErrorContent.OrderNotFound });

            var result = _mapper.Map<OrderGetDto>(order);
            return Ok(new ApiResponse { Success = true, Data = result });
        }

        // POST: api/ManagementOrders/UpdateStatusOrder
        [HttpPut("UpdateStatusOrder/{id}")]
        public async Task<IActionResult> UpdateStatusOrder(Guid id, OrderUpdateStatusDto orderDto)
        {
            if (id != orderDto.OrderId)
                return BadRequest(new ApiResponse { Message = ErrorContent.NotMatchId });

            var order = await _order.FindById(id);
            if (order == null || order.IsDelete)
                return NotFound(new ApiResponse { Message = ErrorContent.OrderNotFound });

            // update order status
            // customer can cancel, refund, receive the order
            // Admin or seller can confirm the order for delivery
            if (order.OrderStatus == SD.OrderOrdered)
            {
                if (orderDto.OrderStatus != SD.OrderDelivering
                    && orderDto.OrderStatus != SD.OrderCanceled)
                    return BadRequest(new ApiResponse { Message = ErrorContent.OrderStatusUnable });
            }
            else
            {
                return BadRequest(new ApiResponse { Message = ErrorContent.OrderStatusUnable });
            }


            order.OrderStatus = orderDto.OrderStatus;
            order.UpdatedAt = DateTime.Now;
            try
            {
                await _order.Update(order);

                var message = "";
                if (orderDto.OrderStatus == SD.OrderDelivering)
                    message = "được xác nhận";
                else if (orderDto.OrderStatus == SD.OrderCanceled)
                {
                    message = "bị huỷ";
                    // add quantity of product in store
                    foreach (var orDetail in order.OrderDetails)
                    {
                        orDetail.Product.Quantity += orDetail.Quantity;
                        orDetail.Product.UpdatedAt = DateTime.Now;
                        await _product.Update(orDetail.Product);
                    }
                    message = "huỷ";
                }

                // add notification for user
                Notification newNotification = new()
                {
                    UserId = order.UserId,
                    Message = $"Đơn hàng của bạn đã {message}. Mã đơn hàng: " + order.OrderId
                };
                await _notification.Add(newNotification);
                // notification signalR
                await _hub.Clients.User(order.UserId.ToString())
                    .SendAsync(SD.NewNotification,
                    _mapper.Map<NotificationGetDto>(newNotification));

                return Ok(new ApiResponse { Success = true });
            }
            catch { return BadRequest(new ApiResponse { Message = ErrorContent.Data }); }
        }
    }
}
