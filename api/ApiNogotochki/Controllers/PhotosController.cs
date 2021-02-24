using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
		public IActionResult Save()
		{
			var photoFile = Request.Form.Files.FirstOrDefault();
			if (photoFile == null)
				return BadRequest("Photo is required");

			byte[] bytes;
			try
			{
				using var bitmap = new Bitmap(photoFile.OpenReadStream());
				using var memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Jpeg);
				bytes = memoryStream.GetBuffer();
			}
			catch (Exception e)
			{
				return BadRequest("Can not save photo. " + e.Message + ". " + e.StackTrace);
			}

			return Ok(photosRepository.Save(bytes));
		}

		[HttpGet("{id}")]
		public void Get([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
			{
				Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return;
			}

			var content = photosRepository.Find(id);
			if (content == null)
			{
				Response.StatusCode = (int) HttpStatusCode.NotFound;
				return;
			}

			Response.StatusCode = (int) HttpStatusCode.OK;
			Response.ContentType = "image/jpeg";
			Response.ContentLength = content.Length;
			Response.Body.WriteAsync(content).GetAwaiter().GetResult();
		}
	}
}