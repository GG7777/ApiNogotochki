using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Filters
{
	public class UserFilter
	{
		public DbUser Filter(DbUser user, bool isMe)
		{
			if (isMe)
				return user;

			user.PhoneNumber = null;
			user.Roles = null;

			return user;
		}
	}
}