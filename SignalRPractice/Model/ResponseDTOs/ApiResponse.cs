using System.Collections.Generic;
using System.Net;

namespace SignalRPractice.Model.ResponseDTOs
{
	public class ApiResponse<T>
	{
		public T Data { get; set; }
		public HttpStatusCode statusCode { get; set; }
		public IEnumerable<ErrorResponse> Error { get; set; } = null;
	}
}
