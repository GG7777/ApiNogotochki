using System;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Items;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("search")]
	public class SearchController : Controller
	{
		private readonly RepositoryContextFactory contextFactory;
		private readonly ServicesRepository servicesRepository;
		private readonly UsersRepository usersRepository;

		public SearchController(RepositoryContextFactory contextFactory,
								ServicesRepository servicesRepository,
								UsersRepository usersRepository)
		{
			this.contextFactory = contextFactory;
			this.servicesRepository = servicesRepository;
			this.usersRepository = usersRepository;
		}

		[HttpGet("services")]
		public IActionResult SearchServices([FromQuery] string? q)
		{
			if (string.IsNullOrWhiteSpace(q))
				return BadRequest($"{nameof(q)} should not be null or white space");

			using var context = contextFactory.Create();

			var serviceIds = context.SearchIndices
									.Where(x => x.TargetType == TargetTypeEnum.Service && Match(x.Value, q))
									.Select(x => x.TargetId)
									.ToArray();

			return Ok(servicesRepository.FindByIds(serviceIds));
		}

		[HttpGet("users")]
		public IActionResult SearchUsers([FromQuery] string? q)
		{
			if (string.IsNullOrWhiteSpace(q))
				return BadRequest($"{nameof(q)} should not be null or white space");

			using var context = contextFactory.Create();

			var userIds = context.SearchIndices
								 .Where(x => x.TargetType == TargetTypeEnum.User && Match(x.Value, q))
								 .Select(x => x.TargetId)
								 .ToArray();

			return Ok(usersRepository.FindByIds(userIds));
		}

		[HttpGet("geolocations")]
		public IActionResult SearchGeolocations([FromQuery(Name = "lat")] string? latitudeString,
												[FromQuery(Name = "lng")] string? longitudeString,
												[FromQuery(Name = "d-lat")] string? deltaLatitudeString,
												[FromQuery(Name = "d-lng")] string? deltaLongitudeString)
		{
			if (!double.TryParse(latitudeString, out var latitude) ||
				!double.TryParse(longitudeString, out var longitude) ||
				!double.TryParse(deltaLatitudeString, out var deltaLatitude) ||
				!double.TryParse(deltaLongitudeString, out var deltaLongitude))
				return BadRequest("Each parameter should be a number");

			using var context = contextFactory.Create();

			var records = context.GeolocationIndices
								 .Where(x => Match(x.Geolocations,
												   latitude,
												   longitude,
												   deltaLatitude,
												   deltaLongitude).Any())
								 .ToArray();
			foreach (var record in records)
				record.Geolocations = Match(record.Geolocations,
											latitude,
											longitude,
											deltaLatitude,
											deltaLongitude);

			return Ok(records);
		}

		private Geolocation[] Match(Geolocation[] geolocations,
									double latitude,
									double longitude,
									double deltaLatitude,
									double deltaLongitude)
		{
			return geolocations.Where(x =>
							   {
								   var lat = double.Parse(x.Latitude);
								   var lng = double.Parse(x.Longitude);

								   return lat <= latitude + deltaLatitude &&
										  lat >= latitude - deltaLatitude &&
										  lng <= longitude + deltaLongitude &&
										  lng >= longitude - deltaLongitude;
							   })
							   .ToArray();
		}

		private bool Match(string value, string query)
		{
			var splitValue = value.Split(",", StringSplitOptions.RemoveEmptyEntries);
			var splitQuery = query.Split(",", StringSplitOptions.RemoveEmptyEntries);

			return splitQuery.Any(x => splitValue.Contains(x));
		}
	}
}