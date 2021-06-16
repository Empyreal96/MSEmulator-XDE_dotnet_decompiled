using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000022 RID: 34
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00004D9A File Offset: 0x00002F9A
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
			{
				throw new InvalidOperationException(Strings.TargetTypeMustBeBool);
			}
			return !(bool)value;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004DC7 File Offset: 0x00002FC7
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
