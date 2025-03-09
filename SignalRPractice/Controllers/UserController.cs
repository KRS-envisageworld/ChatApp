using Microsoft.AspNetCore.Mvc;

using SignalRPractice.Helper;
using SignalRPractice.Model.ResponseDTOs;
using SignalRPractice.Model;
using SignalRPractice.Services;

using System.Threading.Tasks;
using System;
using SignalRPractice.Interfaces;
using SignalRPractice.Model.RequestDTO;

namespace SignalRPractice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(IUser userService) : ControllerBase
	{
		[HttpPost("Register")]
		public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] UserRequestDTO user)
		{
			try
			{
				var newUser = await userService.RegisterAsync(user);
				if (newUser.Length > 0)
				{
					return ResponseHelper.CreateErrorResponse<object>(newUser, System.Net.HttpStatusCode.BadRequest);
				}

				var createdUser = await userService.GetUserByUsername(user.Username);
				UserDTO userDTO = new()
				{
					Username = createdUser.Username,
					Email = createdUser.Email,
					FirstName = createdUser.FirstName,
					LastName = createdUser.LastName,
					Id = createdUser.Id,
					EmailVerified = createdUser.EmailVerified,
					CreatedOn = createdUser.CreatedOn,
				};
				return ResponseHelper.CreateResponse<object>(userDTO, System.Net.HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return ResponseHelper.CreateErrorResponse<object>(["Something went wrong!!!"], System.Net.HttpStatusCode.InternalServerError);
			}

		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<ApiResponse<object>>> GetUser([FromRoute] int userId)
		{
			try
			{
				var newUser = await userService.GetUserByUserId(userId);
				if (newUser == null)
				{
					return ResponseHelper.CreateErrorResponse<object>(["User not found."], System.Net.HttpStatusCode.NotFound);
				}

				UserDTO userDTO = new()
				{
					Username = newUser.Username,
					Email = newUser.Email,
					FirstName = newUser.FirstName,
					LastName = newUser.LastName,
					Id = newUser.Id,
					EmailVerified = newUser.EmailVerified,
					CreatedOn = newUser.CreatedOn,
				};
				return ResponseHelper.CreateResponse<object>(userDTO, System.Net.HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return ResponseHelper.CreateErrorResponse<object>(["Something went wrong!!!"], System.Net.HttpStatusCode.InternalServerError);
			}

		}

		[HttpPost("Login")]
		public async Task<ActionResult<ApiResponse<object>>> Login( string username, string password)
		{
			try
			{
				var token = await userService.ValidateUser(username, password);
				if(token == null)
				{
					return ResponseHelper.CreateErrorResponse<object>(["Username or Password is not valid. Please check and login again."], System.Net.HttpStatusCode.BadRequest);
				}
				return ResponseHelper.CreateResponse<object>(token, System.Net.HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return ResponseHelper.CreateErrorResponse<object>(["Something went wrong!!!"], System.Net.HttpStatusCode.InternalServerError);
			}
		}
	}
}
