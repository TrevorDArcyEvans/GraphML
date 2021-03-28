namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public sealed class NodePage : ReposoitoryItemPageBase
	{
		protected override async Task<IEnumerable<RepositoryItem>> GetRepositoryItems(Repository repo, int pageIndex, int pageSize)
		{
			return await NodeServer.ByOwners(new[] { repo.Id }, pageIndex, pageSize);
		}
	}
}
