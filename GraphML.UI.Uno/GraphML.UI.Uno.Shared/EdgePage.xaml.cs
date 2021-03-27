namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Linq;

	public sealed partial class EdgePage : RepoItemPageBase
	{
		public EdgePage()
		{
			InitializeComponent();
		}

		protected override async void InitialiseUI(Repository repo)
		{
			var repoItems = await EdgeServer.ByOwners(new[] { repo.Id });
			repoItems.ToList()
		  .ForEach(repoItem => MarshallToUI(() => RepositoryItems.Add(repoItem)));
		}
	}
}
