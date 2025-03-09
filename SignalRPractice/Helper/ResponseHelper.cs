using SignalRPractice.Model.ResponseDTOs;

using System.Collections.Generic;
using System.Net;

namespace SignalRPractice.Helper
{
	public class ResponseHelper
	{
		public static ApiResponse<T> CreateResponse<T>(T data, HttpStatusCode statusCode)
		{
			return new ApiResponse<T>
			{
				Data = data,
				statusCode = statusCode
			};
		}	
		public static ApiResponse<T> CreateErrorResponse<T>(string[] errorMessage, HttpStatusCode statusCode)
		{
			List<ErrorResponse> errorList = new List<ErrorResponse>();
			foreach (var message in errorMessage)
			{
				errorList.Add(new ErrorResponse { Message = message });
			}	
			return new ApiResponse<T>
			{
				Error = errorList,
				statusCode = statusCode
			};
		}
	}
}
