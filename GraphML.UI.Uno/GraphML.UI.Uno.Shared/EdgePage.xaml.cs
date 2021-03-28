namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public sealed class EdgePage : ReposoitoryItemPageBase
	{
		protected override async Task<IEnumerable<RepositoryItem>> GetRepositoryItems(Repository repo, int pageIndex, int pageSize)
		{
			return await EdgeServer.ByOwners(new[] { repo.Id }, pageIndex, pageSize);
		}
	}
}
