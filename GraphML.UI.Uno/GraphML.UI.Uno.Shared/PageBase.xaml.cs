using System;
using Windows.UI.Core;

namespace GraphML.UI.Uno
{
	using System.Net.Http;
	using Windows.UI.Xaml.Controls;
	using Autofac;
	using Microsoft.Extensions.Configuration;

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

