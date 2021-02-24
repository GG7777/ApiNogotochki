using System.Collections.Concurrent;
using System.Linq;
using ApiNogotochki.ActionFilters;
using ApiNogotochki.Enums;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/dialogs")]
	public class DialogsController : Controller
	{
		private readonly DialogsRepository dialogsRepository;
		private readonly MessagesRepository messagesRepository;

		private readonly ConcurrentDictionary<string, DbDialog> dialogsCache = new ConcurrentDictionary<string, DbDialog>();

		public DialogsController(DialogsRepository dialogsRepository, MessagesRepository messagesRepository)
		{
			this.dialogsRepository = dialogsRepository;
			this.messagesRepository = messagesRepository;
		}

		[HttpGet]
		[Authorize(UserRoleEnum.User)]
		public IActionResult GetOrCreateDialog([FromQuery(Name = "user-id")] string? userId,
											   [FromQuery(Name = "service-id")] string? serviceId)
		{
			if (string.IsNullOrEmpty(serviceId))
				return BadRequest($"{nameof(DbDialog.ServiceId)} is required");

			if (string.IsNullOrEmpty(userId))
				return BadRequest($"{nameof(DbDialog.UserId)} is required");

			var user = HttpContext.TryGetUser();
			if (user == null)
				throw new InvalidStateException("User should be not null");

			var dialog = new DbDialog
			{
				ServiceId = serviceId,
				UserId = userId
			};

			if (!AuthorizeUserToDialog(dialog, user))
				return BadRequest("You are not authorized to dialog");

			var dbDialog = dialogsRepository.GetOrCreate(new DbDialog
			{
				ServiceId = serviceId,
				UserId = userId
			});

			return Ok(dbDialog);
		}

		[HttpGet("{id}")]
		[Authorize(UserRoleEnum.User)]
		public IActionResult GetDialog([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var dialog = TryGetDialog(id);
			if (dialog == null)
				return NotFound("Dialog not found");

			var user = HttpContext.TryGetUser();
			if (user == null)
				throw new InvalidStateException("User should be not null");

			if (!AuthorizeUserToDialog(dialog, user))
				return NotFound("Dialog not found");

			return Ok(dialog);
		}

		[HttpGet("{id}/messages")]
		[Authorize(UserRoleEnum.User)]
		public IActionResult GetMessages([FromRoute] string? id,
										 [FromQuery] long? timestamp)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var dialog = TryGetDialog(id);
			if (dialog == null)
				return NotFound("Dialog not found");

			var user = HttpContext.TryGetUser();
			if (user == null)
				throw new InvalidStateException("User should be not null");

			if (!AuthorizeUserToDialog(dialog, user))
				return NotFound("Dialog not found");

			return Ok(messagesRepository.GetMessages(id, timestamp ?? -1));
		}

		[HttpPost("{id}/messages")]
		[Authorize(UserRoleEnum.User)]
		public IActionResult SaveMessage([FromRoute] string? id, [FromBody] DbMessage? message)
		{
			if (message == null)
				return BadRequest("Body is required");

			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (string.IsNullOrWhiteSpace(message.Message))
				return BadRequest($"{nameof(DbMessage.Message)} is required");

			if (string.IsNullOrEmpty(message.SenderType))
				return BadRequest($"{nameof(DbMessage.SenderType)} is required");

			if (message.SenderType != TargetTypeEnum.User && message.SenderType != TargetTypeEnum.Service)
				return BadRequest($"Not supported {nameof(DbMessage.SenderType)} = '{message.SenderType}'");

			var dialog = TryGetDialog(id);
			if (dialog == null)
				return NotFound("Dialog not found");

			var user = HttpContext.TryGetUser();
			if (user == null)
				throw new InvalidStateException("User should be not null");

			if (!AuthorizeUserToDialog(dialog, user))
				return NotFound("Dialog not found");

			if (message.SenderType == TargetTypeEnum.Service && !AuthorizeUserToService(dialog, user) ||
				message.SenderType == TargetTypeEnum.User && !AuthorizeUserToSelf(dialog, user))
				return BadRequest("You are not authorized");

			message.DialogId = dialog.Id;
			message.SenderId = message.SenderType switch
			{
				TargetTypeEnum.Service => dialog.ServiceId,
				TargetTypeEnum.User => dialog.UserId,
				_ => throw new InvalidStateException("Validation bug of not supported SenderType")
			};

			return Ok(messagesRepository.Save(message));
		}

		private DbDialog? TryGetDialog(string id)
		{
			if (dialogsCache.TryGetValue(id, out var cachedDialog))
				return cachedDialog;
			var dialog = dialogsRepository.TryGetDialog(id);
			if (dialog != null)
				dialogsCache[id] = dialog;
			return dialog;
		}
		
		private bool AuthorizeUserToDialog(DbDialog dialog, DbUser user)
		{
			return AuthorizeUserToSelf(dialog, user) || AuthorizeUserToService(dialog, user);
		}

		private bool AuthorizeUserToSelf(DbDialog dialog, DbUser user)
		{
			return user.Id == dialog.UserId;
		}

		private bool AuthorizeUserToService(DbDialog dialog, DbUser user)
		{
			return user.ServiceIds?.Contains(dialog.ServiceId) == true;
		}
	}
}