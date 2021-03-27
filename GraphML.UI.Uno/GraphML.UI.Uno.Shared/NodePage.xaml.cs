namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Linq;

	public sealed partial class NodePage : RepoItemPageBase
	{
		public NodePage()
		{
			InitializeComponent();
		}

		protected override async void InitialiseUI(Repository repo)
		{
			var repoItems = await NodeServer.ByOwners(new[] { repo.Id });
			repoItems.ToList()
		  .ForEach(repoItem => MarshallToUI(() => RepositoryItems.Add(repoItem)));
		}
	}
}
