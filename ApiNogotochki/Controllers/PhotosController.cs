using ApiNogotochki.Enums;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("photos")]
	public class PhotosController : Controller
	{
		private readonly PhotosRepository photosRepository;

		public PhotosController(PhotosRepository photosRepository)
		{
			this.photosRepository = photosRepository;
		}

		[HttpPost]
		public IActionResult Save([FromBody] string? path)
		{
			if (path == null)
				return BadRequest("body is required");
			
			var id = photosRepository.Save(PhotoSizeEnum.Original, path);

			return Ok(id);
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string? id)
		{
			if (id == null)
				return BadRequest($"{nameof(id)} is required");

			var path = photosRepository.Find(id, PhotoSizeEnum.Original);

			return Ok(path);
		}
	}
}