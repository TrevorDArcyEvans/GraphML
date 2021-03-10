namespace GraphML.UI.Uno
{
	using System;
	using Windows.UI.Xaml.Controls;

	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();

			dp.Date = new DateTime(2006, 2, 20);
		}

		private void OnClick(object sender, object args)
		{
			var dt = DateTime.Now.ToString("O");
			txt.Text = dt;
		}

		public void Menu_OnClick(object sender, object args)
		{
			txt.Text = "Reset";
		}
	}
}
