namespace GraphML.UI.Uno.Converters
{
	using System;
	using Windows.UI.Xaml.Data;

	public sealed class ObjectToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return !(value is null);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
