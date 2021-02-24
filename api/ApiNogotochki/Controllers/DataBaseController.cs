using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("data-base")]
	public class DataBaseController : Controller
	{
		private readonly RepositoryContextFactory contextFactory;

		public DataBaseController(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		[HttpGet("delete-create")]
		public IActionResult DeleteAndCreate()
		{
			using var context = contextFactory.Create();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			return Ok("Готово, хозяин!");
		}

		[HttpGet("create")]
		public IActionResult Create()
		{
			using var context = contextFactory.Create();
			context.Database.EnsureCreated();

			return Ok("Готово, хозяин!");
		}

		[HttpGet("delete")]
		public IActionResult Delete()
		{
			using var context = contextFactory.Create();
			context.Database.EnsureDeleted();

			return Ok("Готово, хозяин!");
		}
	}
}