using System.Linq;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

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
			return user.Id
					   .Select(x => (x + '0') % char.MaxValue)
					   .Select(x => x.ToString())
					   .Aggregate((acc, x) => acc + x);
		}

		public DbUser? TryGetUser(string token)
		{
			var userId = token.Select(x => (x - '0') % char.MaxValue)
							  .Select(x => x.ToString())
							  .Aggregate((acc, x) => acc + x);
			return usersRepository.FindById(userId);
		}
	}
}