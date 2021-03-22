namespace GraphML.UI.Uno
{
	using System.Collections.Generic;

	public sealed class BreadcrumbTrail : Dictionary<string, object>
	{
		public string Token
		{
			get => (string)this[nameof(Token)];
			set => this[nameof(Token)] = value;
		}

		public Organisation SelectedOrganisation
		{
			get => (Organisation)this[nameof(SelectedOrganisation)];
			set => this[nameof(SelectedOrganisation)] = value;
		}

		public RepositoryManager SelectedRepositoryManager
		{
			get => (RepositoryManager)this[nameof(SelectedRepositoryManager)];
			set => this[nameof(SelectedRepositoryManager)] = value;
		}

		public Repository SelectedRepository
		{
			get => (Repository)this[nameof(SelectedRepository)];
			set => this[nameof(SelectedRepository)] = value;
		}

		public Graph SelectedGraph
		{
			get => (Graph)this[nameof(SelectedGraph)];
			set => this[nameof(SelectedGraph)] = value;
		}
	}
}
