using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001A RID: 26
	public class BoolToVisibilityConverter : IValueConverter
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00004A2C File Offset: 0x00002C2C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string value2 = parameter as string;
			bool flag = string.IsNullOrEmpty(value2) || bool.Parse(value2);
			return ((bool)value == flag) ? Visibility.Visible : Visibility.Collapsed;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004A65 File Offset: 0x00002C65
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
