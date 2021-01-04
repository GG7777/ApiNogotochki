using System.Collections.Generic;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using JWT.Algorithms;
using JWT.Builder;

#nullable enable

namespace ApiNogotochki.Manager
{
	public class JwtTokenProvider
	{
		private readonly UsersRepository usersRepository;

		public JwtTokenProvider(UsersRepository usersRepository)
		{
			this.usersRepository = usersRepository;
		}

		public string GetToken(DbUser user)
		{
			return new JwtBuilder().WithSecret("qwfRJLkwj321OFAI9mmasawq")
								   .WithAlgorithm(new HMACSHA256Algorithm())
								   .AddClaim("userId", user.Id)
								   .Encode();
		}

		public DbUser? TryGetUser(string token)
		{
			if (string.IsNullOrEmpty(token))
				return null;

			string? userId;
			try
			{
				userId = (string) new JwtBuilder().WithSecret("qwfRJLkwj321OFAI9mmasawq")
												  .WithAlgorithm(new HMACSHA256Algorithm())
												  .Decode<IDictionary<string, object>>(token)
												  ["userId"];
			}
			catch
			{
				return null;
			}

			if (userId == null)
				return null;
			
			return usersRepository.FindById(userId);
		}
	}
}