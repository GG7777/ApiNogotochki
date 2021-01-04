using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiNogotochki.Filters
{
	public class StringBodyAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;
			var length = (int)request.ContentLength!.Value;
			var buffer = new byte[length];

			// надо ли тут ConfigureAwait(false)?
			var readLength = request.Body.ReadAsync(buffer).GetAwaiter().GetResult();
			if (readLength != length)
				throw new Exception($"Read length = {readLength} should be equal to length = {length}");

			context.ActionArguments["stringBody"] = Encoding.UTF8.GetString(buffer);
		}
	}
}