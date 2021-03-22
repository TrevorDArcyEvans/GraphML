namespace GraphML.UI.Uno
{
	using Autofac;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Net.Http;
	using Windows.UI.Core;
	using Windows.UI.Xaml.Controls;

	public partial class PageBase : Page
	{
		protected readonly IConfigurationRoot _config;
		protected readonly HttpMessageHandler _innerHandler;
		protected BreadcrumbTrail _navArgs = new BreadcrumbTrail();

		public PageBase()
		{
			this.InitializeComponent();
			_config = App.Container.Resolve<IConfigurationRoot>();
#if __WASM__
			_innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
			_innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
#endif
		}

		protected async void MarshallToUI(Action action)
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
		}
	}
}

