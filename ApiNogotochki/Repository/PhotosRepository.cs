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

		public string Save(string size, string path)
		{
			using var context = contextFactory.Create();
			var photo = new DbPhoto
			{
				Id = Guid.NewGuid().ToString(),
				Size = size,
				Path = path,
			};
			context.Photos.Add(photo);
			context.SaveChanges();
			return photo.Id;
		}

		public string? Find(string id, string size)
		{
			using var context = contextFactory.Create();
			return context.Photos.SingleOrDefault(x => x.Id == id && x.Size == size)?.Path;
		}
	}
}