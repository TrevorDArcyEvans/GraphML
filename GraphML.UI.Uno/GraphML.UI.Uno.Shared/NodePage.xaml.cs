namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public sealed class NodePage : ReposoitoryItemPageBase
	{
		protected override async Task<IEnumerable<RepositoryItem>> GetRepositoryItems(Repository repo)
		{
			return await NodeServer.ByOwners(new[] { repo.Id }, 1, 20); //TODO paging
		}
	}
}
