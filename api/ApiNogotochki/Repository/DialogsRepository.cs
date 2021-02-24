using System;
using System.Linq;
using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class DialogsRepository
	{
		private readonly RepositoryContextFactory contextFactory;

		public DialogsRepository(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public DbDialog GetOrCreate(DbDialog dialog)
		{
			using var context = contextFactory.Create();
			var existingDialog = context.Dialogs.SingleOrDefault(x => x.UserId == dialog.UserId &&
																	  x.ServiceId == dialog.ServiceId);
			if (existingDialog == null)
			{
				dialog.Id = Guid.NewGuid().ToString();
				context.Dialogs.Add(dialog);
				context.SaveChanges();
			}

			return dialog;
		}

		public DbDialog? TryGetDialog(string id)
		{
			using var context = contextFactory.Create();
			return context.Dialogs.SingleOrDefault(x => x.Id == id);
		}

		public DbDialog[] GetDialogsFromUser(string userId)
		{
			using var context = contextFactory.Create();
			return context.Dialogs.Where(x => x.UserId == userId).ToArray();
		}

		public DbDialog[] GetDialogsFromService(string serviceId)
		{
			using var context = contextFactory.Create();
			return context.Dialogs.Where(x => x.ServiceId == serviceId).ToArray();
		}
	}
}