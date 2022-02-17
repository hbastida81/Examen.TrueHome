using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Examen.Tranversal.Common
{
	public class Response<T>
	{
		public T Data { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
	}
}
