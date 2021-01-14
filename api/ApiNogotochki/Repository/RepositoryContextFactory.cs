using System;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class RepositoryContextFactory
	{
		private static readonly string ConnectionString =
			Environment.GetEnvironmentVariable("API_NOGOTOCHKI_POSTGRES_CONNECTION_STRING") ??
			"Host=localhost;Port=5432;Database=api_nogotochki;Username=api_nogotochki;Password=password";

		public RepositoryContext Create()
		{
			return new RepositoryContext(ConnectionString);
		}
	}
}