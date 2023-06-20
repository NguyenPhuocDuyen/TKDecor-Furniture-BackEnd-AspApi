﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using BE_TKDecor.Core.Dtos.UserAddress;
using BE_TKDecor.Core.Response;

namespace BE_TKDecor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAddressesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddressesController(IMapper mapper,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
        }

        // GET: api/UserAddresses/GetUserAddresses
        [HttpGet("GetUserAddresses")]
        public async Task<IActionResult> GetUserAddresses()
        {
            var user = await GetUser();
            if (user == null)
                return NotFound(new ApiResponse { Message = ErrorContent.UserNotFound });

            var list = await _userAddressRepository.GetByUserId(user.UserId);
            var result = _mapper.Map<List<UserAddressGetDto>>(list);

            return Ok(new ApiResponse { Success = true, Data = result });
        }

        [HttpPost("SetDefault")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var user = await GetUser();
            if (user == null)
                return NotFound(new ApiResponse { Message = ErrorContent.UserNotFound });

            var address = await _userAddressRepository.FindById(id);
            if (address == null)
                return NotFound(new ApiResponse { Message = ErrorContent.AddressNotFound });

            try
            {
                await _userAddressRepository.SetDefault(user.UserId, id);
                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = ErrorContent.Data }); }

        }

        // POST: api/UserAddresses
        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserAddressCreateDto userAddressDto)
        {
            var user = await GetUser();
            if (user == null)
                return NotFound(new ApiResponse { Message = ErrorContent.UserNotFound });

            UserAddress newAddress = _mapper.Map<UserAddress>(userAddressDto);
            newAddress.UserId = user.UserId;
            try
            {
                await _userAddressRepository.Add(newAddress);

                var listAddress = await _userAddressRepository.GetByUserId(user.UserId);
                if (listAddress.Count <= 1)
                {
                    await _userAddressRepository.SetDefault(user.UserId, null);
                }

                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = ErrorContent.Data }); }
        }

        // DELETE: api/UserAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAddress(int id)
        {
            var userAddress = await _userAddressRepository.FindById(id);
            if (userAddress == null)
                return NotFound(new ApiResponse { Message = ErrorContent.AddressNotFound });

            try
            {
                await _userAddressRepository.Delete(userAddress);
                return NoContent();
            }
            catch { return BadRequest(new ApiResponse { Message = ErrorContent.Data }); }
        }

        private async Task<User?> GetUser()
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "UserId"))
            {
                var userId = currentUser?.Claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;
                // get user by user id
                if (userId != null)
                    return await _userRepository.FindById(int.Parse(userId));
            }
            return null;
        }
    }
}