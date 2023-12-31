﻿using BE_TKDecor.Core.Dtos.ProductReview;
using BE_TKDecor.Core.Response;
using BE_TKDecor.Service.IService;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace BE_TKDecor.Service
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly TkdecorContext _context;
        private readonly ApiResponse _response;

        public ProductReviewService(TkdecorContext context)
        {
            _context = context;
            _response = new ApiResponse();
        }

        // user make review product when bought product
        public async Task<ApiResponse> ReviewProduct(string? userId, ProductReviewCreateDto dto)
        {
            var orderDetail = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.Product)
                .Include(x => x.ProductReview)
                .FirstOrDefaultAsync(x => x.OrderDetailId == dto.OrderDetailId
                                    && x.Order.UserId.ToString() == userId);

            if (orderDetail is null)
            {
                _response.Message = "Không tìm thấy order item";
                return _response;
            }

            if (orderDetail.Order.OrderStatus != SD.OrderReceived)
            {
                _response.Message = "Không được phép đánh giá";
                return _response;
            }

            orderDetail.ProductReview ??= new ProductReview();
            orderDetail.ProductReview.IsDelete = false;
            orderDetail.ProductReview.UpdatedAt = DateTime.Now;
            orderDetail.ProductReview.Rate = dto.Rate;
            orderDetail.ProductReview.Description = dto.Description;
            try
            {
                _context.OrderDetails.Update(orderDetail);

                await _context.SaveChangesAsync();
                _response.Success = true;
            }
            catch { _response.Message = ErrorContent.Data; }
            return _response;
        }
    }
}
