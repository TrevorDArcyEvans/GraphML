namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class OrganisationPage : PageBase
	{
		private OrganisationServer _orgServer;

		public OrganisationPage() :
			base()
		{
			InitializeComponent();
		}

		public ObservableCollection<Organisation> Organisations { get; set; } = new ObservableCollection<Organisation>();

		private Organisation _selectedOrganisation;

		public Organisation SelectedOrganisation
		{
			get => _selectedOrganisation;
			set => SetProperty(ref _selectedOrganisation, value);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise();
		}

		private async void Initialise()
		{
			_orgServer = new OrganisationServer(_config, _navArgs.Token, _innerHandler);

			await LoadItems();
		}

		private async Task LoadItems()
		{
			Organisations.Clear();

			var orgs = await _orgServer.GetAll(_pageIndex, PageSize);
			orgs.ToList()
		  .ForEach(org => Organisations.Add(org));
		}

		protected async void Previous_Click(object sender, object args)
		{
			if (_pageIndex > 1)
			{
				_pageIndex--;
			}

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems();
		}

		protected async void Next_Click(object sender, object args)
		{
			_pageIndex++;

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems();
		}

		private void Organisation_Click(object sender, object args)
		{
			_navArgs.SelectedOrganisation = SelectedOrganisation;
			Frame.Navigate(typeof(RepositoryManagerPage), _navArgs);
		}

		private void Logout_Click(object sender, object args)
		{
			Frame.Navigate(typeof(LoginPage));
		}
	}
}
