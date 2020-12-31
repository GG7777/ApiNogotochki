using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Route("data-base")]
	[Controller]
	public class DataBaseController : Controller
	{
		[HttpGet("delete-create")]
		public IActionResult DeleteAndCreate()
		{
			var context = new RepositoryContext();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			return Ok("Готово, господин!");
		}
		
		[HttpGet("create")]
		public IActionResult Create()
		{
			new RepositoryContext().Database.EnsureCreated();

			return Ok("Готово, господин!");
		}
		
		[HttpGet("delete")]
		public IActionResult Delete()
		{
			new RepositoryContext().Database.EnsureDeleted();

			return Ok("Готово, господин!");
		}
	}
}