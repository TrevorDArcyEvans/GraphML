namespace GraphML.UI.Uno
{
	using Autofac;
	using GraphML.UI.Uno.Annotations;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Net.Http;
	using System.Runtime.CompilerServices;
	using Windows.UI.Core;
	using Windows.UI.Xaml.Controls;

	public partial class PageBase : Page, INotifyPropertyChanged
	{
    protected const int PageSize = 10;
    protected int _pageIndex = 1;

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

		public event PropertyChangedEventHandler PropertyChanged;

		protected async void MarshallToUI(Action action)
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}
	}
}

