using System;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class UsersRepository
	{
		public DbUser GetOrCreate(string phoneNumber)
		{
			var context = new RepositoryContext();
			var existingUser = context.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
			if (existingUser != null)
				return existingUser;
			
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
			var context = new RepositoryContext();
			return context.Users.SingleOrDefault(x => x.Id == id);
		}
	}
}