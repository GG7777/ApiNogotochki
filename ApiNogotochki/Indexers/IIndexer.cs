#nullable enable

namespace ApiNogotochki.Indexers
{
	public interface IIndexer<TType>
	{
		void Index(TType obj);
	}
}