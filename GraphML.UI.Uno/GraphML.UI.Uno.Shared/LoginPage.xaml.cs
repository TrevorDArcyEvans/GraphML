namespace GraphML.UI.Uno
{
	using Autofac;
	using GraphML.Common;
	using IdentityModel.Client;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Net.Http;
	using System.Security.Authentication;
	using Windows.UI.Xaml.Controls;

	public sealed partial class LoginPage : Page
	{
		private readonly IConfigurationRoot _config;

		public LoginPage()
		{
			InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		private async void Login_Click(object sender, object args)
		{
#if __WASM__
			var innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
#endif
			var client = new HttpClient(innerHandler)
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

			Frame.Navigate(typeof(MainPage), response.AccessToken);
		}
	}
}
