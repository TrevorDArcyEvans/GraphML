using Windows.UI.Xaml;

namespace GraphML.UI.Uno
{
	using GraphML.Common;
	using IdentityModel.Client;
	using System;
	using System.Net.Http;
	using System.Security.Authentication;

	public sealed partial class LoginPage : PageBase
	{
		public LoginPage() :
			base()
		{
			InitializeComponent();

#if __LINUX__
      _userName.Text = "alice";
			_password.Password = "Pass123$";
      OnPropertyChanged(nameof(_userName));
      OnPropertyChanged(nameof(_password));
#endif      
		}

		private async void Login_Click(object sender, object args)
		{
			var client = new HttpClient(_innerHandler)
			{
				BaseAddress = new Uri(_config.IDENTITY_SERVER_BASE_URL())
			};
			var disco = await client.GetDiscoveryDocumentAsync();
			if (disco.IsError)
			{
				_errors.Text = disco.Error;
				_errors.Visibility = Visibility.Visible;
				return;
			}

			var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
			{
				Address = disco.TokenEndpoint,

				ClientId = _config.IDENTITY_SERVER_CLIENT_ID(),
				ClientSecret = _config.IDENTITY_SERVER_CLIENT_SECRET(),

				UserName = _userName.Text,
				Password = _password.Password
			});
			if (response.IsError)
			{
				_errors.Text = response.Error;
				_errors.Visibility = Visibility.Visible;
				return;
			}

			_navArgs.Token = response.AccessToken;
			Frame.Navigate(typeof(OrganisationPage), _navArgs);
		}
	}
}
