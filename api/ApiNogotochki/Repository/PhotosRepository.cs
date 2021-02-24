using System;
using System.Linq;
using ApiNogotochki.Model;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class PhotosRepository
	{
		private readonly RepositoryContextFactory contextFactory;

		public PhotosRepository(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public string Save(byte[] bytes)
		{
			using var context = contextFactory.Create();
			var photo = new DbPhoto
			{
				Id = Guid.NewGuid().ToString(),
				Content = bytes,
			};
			context.Photos.Add(photo);
			context.SaveChanges();
			return photo.Id;
		}

		public byte[]? Find(string id)
		{
			using var context = contextFactory.Create();
			return context.Photos.SingleOrDefault(x => x.Id == id)?.Content;
		}
	}
}