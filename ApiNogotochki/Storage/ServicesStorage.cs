// using System.Collections.Generic;
// using System.Linq;
// using ApiNogotochki.Model;
// using ApiNogotochki.Repository;
//
// #nullable enable
//
// namespace ApiNogotochki.Storage
// {
// 	public class ServicesStorage
// 	{
// 		private readonly RepositoryContextFactory contextFactory;
//
// 		public ServicesStorage(RepositoryContextFactory contextFactory)
// 		{
// 			this.contextFactory = contextFactory;
// 		}
// 		
// 		public void Write(DbService dbService)
// 		{
// 			using var context = contextFactory.Create();
// 			context.Services.Add(dbService);
// 			context.SaveChanges();
// 		}
// 		
// 		public DbService? TryRead(string id)
// 		{
// 			using var context = contextFactory.Create();
// 			return context.Services.SingleOrDefault(x => x.Id == id);
// 		}
// 		
// 		public IEnumerable<DbService> Read(string[] ids)
// 		{
// 			using var context = contextFactory.Create();
// 			return context.Services.Where(x => ids.Contains(x.Id));
// 		}
// 	}
// }