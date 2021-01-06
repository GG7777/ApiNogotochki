using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiNogotochki.Filters
{
	public class ForbidResult : IActionResult
	{
		public async Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
			await context.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Сюда не хади"));
		}
	}
}