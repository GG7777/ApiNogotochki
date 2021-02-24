using System;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Indexers;
using ApiNogotochki.Model;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class UsersRepository
	{
		private readonly RepositoryContextFactory contextFactory;

		public UsersRepository(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public DbUser GetOrCreate(string phoneNumber)
		{
			using var context = contextFactory.Create();

			var existingUser = context.Users.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
			if (existingUser != null)
			{
				if (existingUser.IsRemoved)
				{
					existingUser.IsRemoved = false;
					context.SaveChanges();
				}

				return existingUser;
			}

			var id = Guid.NewGuid().ToString();
			var createdUser = new DbUser
			{
				Id = id,
				Nickname = id,
				PhoneNumber = phoneNumber,
				Roles = new[] {UserRoleEnum.User},
				IsRemoved = false
			};

			context.Users.Add(createdUser);
			context.SaveChanges();

			return createdUser;
		}

		public DbUser? TryUpdate(DbUser user)
		{
			using var context = contextFactory.Create();

			var existingUser = context.Users
									  .AsNoTrackingWithIdentityResolution()
									  .SingleOrDefault(x => x.Id == user.Id && !x.IsRemoved);
			if (existingUser == null)
				return null;

			user.Id = existingUser.Id;
			user.Nickname = existingUser.Nickname;
			user.PhoneNumber = existingUser.PhoneNumber;
			user.Roles = existingUser.Roles;
			user.ServiceIds = existingUser.ServiceIds;
			user.IsRemoved = existingUser.IsRemoved;

			context.Update(user);
			context.SaveChanges();

			return user;
		}

		public DbUser? TryUpdatePhoneNumber(string id, string phoneNumber)
		{
			using var context = contextFactory.Create();

			var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (user == null)
				return null;

			var phoneNumberUser = context.Users.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
			if (phoneNumberUser != null)
				return phoneNumberUser.Id == id
						   ? phoneNumberUser
						   : null;

			user.PhoneNumber = phoneNumber;

			context.SaveChanges();

			return user;
		}

		public DbUser? TryUpdateNickname(string id, string nickname)
		{
			using var context = contextFactory.Create();

			var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (user == null)
				return null;

			var nicknameUser = context.Users.SingleOrDefault(x => x.Nickname == nickname);
			if (nicknameUser != null)
				return nicknameUser.Id == id
						   ? nicknameUser
						   : null;

			user.Nickname = nickname;

			context.SaveChanges();

			return user;
		}

		public DbUser? TryUpdateServiceIds(string id, string[]? serviceIds)
		{
			using var context = contextFactory.Create();

			var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (user == null)
				return null;

			user.ServiceIds = serviceIds;

			context.SaveChanges();

			return user;
		}

		public DbUser? TryRemove(DbUser user)
		{
			using var context = contextFactory.Create();

			var existingUser = context.Users.SingleOrDefault(x => x.Id == user.Id && !x.IsRemoved);
			if (existingUser == null)
				return null;

			existingUser.IsRemoved = true;

			context.SaveChanges();

			return existingUser;
		}

		public DbUser? FindById(string id)
		{
			using var context = contextFactory.Create();
			return context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
		}

		public DbUser[] FindByIds(params string[] ids)
		{
			if (!ids.Any())
				return new DbUser[0];

			using var context = contextFactory.Create();
			return context.Users
						  .Where(x => ids.Any(z => z == x.Id) && !x.IsRemoved)
						  .ToArray();
		}
	}
}