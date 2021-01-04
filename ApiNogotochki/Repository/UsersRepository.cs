using System;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Model;

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

			var createdUser = context.Users.Add(new DbUser
			{
				Id = Guid.NewGuid().ToString(),
				Roles = new[] {UserRoleEnum.User},
				IsRemoved = false,
				PhoneNumber = phoneNumber,
			});
			
			context.SaveChanges();
			
			return createdUser.Entity;
		}

		public DbUser? FindById(string id)
		{
			using var context = contextFactory.Create();
			return context.Users
						  .Where(x => !x.IsRemoved)
						  .SingleOrDefault(x => x.Id == id);
		}
	}
}