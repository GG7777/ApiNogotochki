using System;
using System.Globalization;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Items;
using ApiNogotochki.Repository;
using ApiNogotochki.Exceptions;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/search")]
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

		[HttpGet("masters")]
		public IActionResult SearchMasters([FromQuery] int? last)
		{
			if (last == null)
				return BadRequest($"{nameof(last)} is required");
			if (last < 0 || last > 100)
				return BadRequest($"{nameof(last)} should be in range [0, 100]");
			
			var count = servicesRepository.GetCount(SearchTypeEnum.Master);
			var offset = count >= last.Value ? count - last.Value : 0;
			var take = last.Value;
			return Ok(servicesRepository.FindBySearchType(SearchTypeEnum.Master, offset, take));
		}

		[HttpGet("models")]
		public IActionResult SearchModels([FromQuery] int? last)
		{
			if (last == null)
				return BadRequest($"{nameof(last)} is required");
			if (last < 0 || last > 100)
				return BadRequest($"{nameof(last)} should be in range [0, 100]");
			
			var count = servicesRepository.GetCount(SearchTypeEnum.Model);
			var offset = count >= last.Value ? count - last.Value : 0;
			var take = last.Value;
			return Ok(servicesRepository.FindBySearchType(SearchTypeEnum.Model, offset, take));
		}

		[HttpGet("services")]
		public IActionResult SearchServices([FromQuery] string? q)
		{
			if (string.IsNullOrWhiteSpace(q))
				return BadRequest($"{nameof(q)} should not be null or white space");

			using var context = contextFactory.Create();

			var serviceIds = context.SearchIndices
									.Where(x => x.TargetType == TargetTypeEnum.Service && x.Value != null)
									.AsEnumerable()
									.Where(x => Match(x.Value, q))
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
								 .Where(x => x.TargetType == TargetTypeEnum.User && x.Value != null)
								 .AsEnumerable()
								 .Where(x => Match(x.Value, q))
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
			if (!TryParseDouble(latitudeString, out var latitude) ||
				!TryParseDouble(longitudeString, out var longitude) ||
				!TryParseDouble(deltaLatitudeString, out var deltaLatitude) ||
				!TryParseDouble(deltaLongitudeString, out var deltaLongitude))
				return BadRequest("Each parameter should be a number");

			using var context = contextFactory.Create();

			var records = context.GeolocationIndices
								 .AsEnumerable()
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

		private bool TryParseDouble(string? str, out double value)
		{
			return double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		private Geolocation[] Match(Geolocation[] geolocations,
									double latitude,
									double longitude,
									double deltaLatitude,
									double deltaLongitude)
		{
			return geolocations.Where(x =>
							   {
								   if (!TryParseDouble(x.Latitude, out var lat) ||
								   	   !TryParseDouble(x.Longitude, out var lng))
										throw new InvalidStateException("Can not parse latitude or longitude");

								   return lat <= latitude + deltaLatitude &&
										  lat >= latitude - deltaLatitude &&
										  lng <= longitude + deltaLongitude &&
										  lng >= longitude - deltaLongitude;
							   })
							   .ToArray();
		}

		private bool Match(string value, string query)
		{
			var splitValue = value.Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries);
			var splitQuery = query.Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries);

			return splitQuery.Any(x => splitValue.Contains(x));
		}
	}
}