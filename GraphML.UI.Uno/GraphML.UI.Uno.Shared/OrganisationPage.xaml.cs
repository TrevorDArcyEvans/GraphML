namespace GraphML.UI.Uno
{
	using System.Collections.ObjectModel;
	using System.Linq;
  using Windows.UI.Xaml.Navigation;
  using GraphML.UI.Uno.Server;

	public sealed partial class OrganisationPage : PageBase
	{
		public OrganisationPage() :
			base()
		{
			InitializeComponent();
		}

		public ObservableCollection<Organisation> Organisations { get; set; } = new ObservableCollection<Organisation>();
		public Organisation SelectedOrganisation { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise();
		}

		private async void Initialise()
		{
			var orgServer = new OrganisationServer(_config, _navArgs.Token, _innerHandler);
			var orgs = await orgServer.GetAll();
			orgs.ToList()
		  .ForEach(org => Organisations.Add(org));
		}

		private void Logout_Click(object sender, object args)
		{
			Frame.Navigate(typeof(LoginPage));
		}

		private void Organisation_Click(object sender, object args)
		{
			_navArgs.SelectedOrganisation = SelectedOrganisation;
			Frame.Navigate(typeof(RepositoryManagerPage), _navArgs);
		}
	}
}
