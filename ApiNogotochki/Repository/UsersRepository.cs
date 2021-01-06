using System;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Indexers;
using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class UsersRepository
	{
		private readonly RepositoryContextFactory contextFactory;
		private readonly Indexer indexer;

		public UsersRepository(RepositoryContextFactory contextFactory, Indexer indexer)
		{
			this.contextFactory = contextFactory;
			this.indexer = indexer;
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
				IsRemoved = false,
			};

			context.Users.Add(createdUser);
			context.SaveChanges();
			
			indexer.Index(createdUser);

			return createdUser;
		}

		public DbUser? TryUpdate(DbUser user)
		{
			using var context = contextFactory.Create();

			var existingUser = context.Users.SingleOrDefault(x => x.Id == user.Id && !x.IsRemoved);
			if (existingUser == null)
				return null;

			existingUser.Description = user.Description;
			existingUser.Geolocations = user.Geolocations;
			existingUser.Name = user.Name;
			existingUser.AvatarId = user.AvatarId;
			existingUser.SocialNetworks = user.SocialNetworks;

			context.SaveChanges();
			
			indexer.Index(existingUser);

			return existingUser;
		}

		public DbUser? TryUpdatePhoneNumber(string id, string phoneNumber)
		{
			using var context = contextFactory.Create();

			var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (user == null)
				return null;

			if (context.Users.SingleOrDefault(x => x.PhoneNumber == phoneNumber) != null)
				return null;
			
			user.PhoneNumber = phoneNumber;

			context.SaveChanges();
			
			indexer.Index(user);

			return user;
		}

		public DbUser? TryUpdateNickname(string id, string nickname)
		{
			using var context = contextFactory.Create();

			var user = context.Users.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (user == null)
				return null;

			if (context.Users.SingleOrDefault(x => x.Nickname == nickname) != null)
				return null;

			user.Nickname = nickname;

			context.SaveChanges();
			
			indexer.Index(user);

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
	}
}