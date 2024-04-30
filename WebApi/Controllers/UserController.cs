﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;
using WebApi.ServiceLayer.JwtLayer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("ByUsername/{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetByUserName(string username)
        {
            var user = await _userService.GetByUserName(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {
            await _userService.Create(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut]
        public async Task<IActionResult> Update(User model)
        {
            try
            {
                await _userService.Update(model);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterDto request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required fields." });
            }

            try
            {
                await _userService.SignUp(request.Username, request.Password, request.Email);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong! Please try again!" });
            }
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(UserDto request)
        {
            try
            {
                var user = await _userService.LogIn(request.Username, request.Password);
                if (user == null)
                    return BadRequest(new { message = "Invalid username or password." });
                if (user.Role != null)
                {
                    var token = _jwtService.GetJWT(user.Username, user.Role);
                    // Return more secure token or user info here.
                    return Ok(token);
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return Ok();
        }
    }
}
