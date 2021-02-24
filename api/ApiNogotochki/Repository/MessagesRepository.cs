using System;
using System.Linq;
using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class MessagesRepository
	{
		private readonly RepositoryContextFactory contextFactory;

		public MessagesRepository(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public DbMessage Save(DbMessage message)
		{
			message.Id = Guid.NewGuid().ToString();
			message.Timestamp = DateTime.UtcNow.Ticks;

			using var context = contextFactory.Create();
			context.Messages.Add(message);

			context.SaveChanges();

			return message;
		}

		public DbMessage[] GetMessages(string dialogId, long timestamp)
		{
			using var context = contextFactory.Create();
			return context.Messages
						  .Where(x => x.DialogId == dialogId &&
									  x.Timestamp > timestamp)
						  .ToArray();
		}
	}
}