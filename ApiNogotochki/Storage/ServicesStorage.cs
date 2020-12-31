using System.Collections.Generic;
using System.Linq;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Storage
{
	public class ServicesStorage
	{
		public void Write(DbService dbService)
		{
			using var context = new RepositoryContext();
			context.ServiceElements.Add(dbService);
			context.SaveChanges();
		}
		
		public DbService? TryRead(string id)
		{
			using var context = new RepositoryContext();
			return context.ServiceElements.SingleOrDefault(x => x.Id == id);
		}
		
		public IEnumerable<DbService> Read(string[] ids)
		{
			using var context = new RepositoryContext();
			return context.ServiceElements.Where(x => ids.Contains(x.Id));
		}
	}
}