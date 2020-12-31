using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("users")]
	public class UsersController : ControllerBase
	{
		private readonly UsersRepository usersRepository;

		public UsersController(UsersRepository usersRepository)
		{
			this.usersRepository = usersRepository;
		}

		// [HttpPut("/")]
		// public IActionResult Update([FromBody] DbUser user)
		// {
		// 	
		// }
		
		[HttpGet]
		public IActionResult Get([FromQuery] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var user = usersRepository.FindById(id);
			if (user == null)
				return BadRequest($"User with id = '{id}' not found");

			return Ok(user);
		}
	}
}