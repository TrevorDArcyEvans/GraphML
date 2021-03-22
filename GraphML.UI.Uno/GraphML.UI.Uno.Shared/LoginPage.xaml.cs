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
				throw new HttpRequestException(disco.Error);
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
				throw new AuthenticationException(response.Error);
			}

			_navArgs.Token = response.AccessToken;
			Frame.Navigate(typeof(OrganisationPage), _navArgs);
		}
	}
}
