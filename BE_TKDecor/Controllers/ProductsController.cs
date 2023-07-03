﻿using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using BE_TKDecor.Core.Response;
using BE_TKDecor.Core.Dtos.Product;
using AutoMapper;

namespace BE_TKDecor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _product;

        public ProductsController(IMapper mapper,
            IProductRepository product)
        {
            _mapper = mapper;
            _product = product;
        }

        // GET: api/Products/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _product.GetAll();
            list = list.Where(x => x.IsDelete == false)
                    .OrderByDescending(x => x.UpdatedAt)
                    .ToList();
            // paging skip 12*0 and take 12 after skip
            //list = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var result = _mapper.Map<List<ProductGetDto>>(list);
            return Ok(new ApiResponse { Success = true, Data = result });
        }

        // GET: api/Products/FeaturedProducts
        [HttpGet("FeaturedProducts")]
        public async Task<IActionResult> FeaturedProducts()
        {
            var products = await _product.GetAll();
            products = products.Where(x => x.IsDelete == false).ToList();
            var sort = products
                .OrderByDescending(x => x.OrderDetails.Sum(x => x.Quantity))
                .Take(9)
                .ToList();

            var result = _mapper.Map<List<ProductGetDto>>(sort);

            return Ok(new ApiResponse { Success = true, Data = result });
        }

        // GET: api/Products/GetById/5
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _product.FindById(id);

            if (product == null || product.IsDelete)
                return NotFound(new ApiResponse { Message = ErrorContent.ProductNotFound });

            var result = _mapper.Map<ProductGetDto>(product);
            return Ok(new ApiResponse { Success = true, Data = result });
        }

        // GET: api/Products/GetBySlug/5
        [HttpGet("GetBySlug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var product = await _product.FindBySlug(slug);

            if (product == null || product.IsDelete)
                return NotFound(new ApiResponse { Message = ErrorContent.ProductNotFound });

            var result = _mapper.Map<ProductGetDto>(product);
            return Ok(new ApiResponse { Success = true, Data = result });
        }
    }
}
