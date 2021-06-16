using System;

namespace System.Management.Automation
{
	// Token: 0x02000117 RID: 279
	public class ConvertThroughString : PSTypeConverter
	{
		// Token: 0x06000EFC RID: 3836 RVA: 0x0005299E File Offset: 0x00050B9E
		public override bool CanConvertFrom(object sourceValue, Type destinationType)
		{
			return !(sourceValue is string);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000529AC File Offset: 0x00050BAC
		public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			string valueToConvert = (string)LanguagePrimitives.ConvertTo(sourceValue, typeof(string), formatProvider);
			return LanguagePrimitives.ConvertTo(valueToConvert, destinationType, formatProvider);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x000529D8 File Offset: 0x00050BD8
		public override bool CanConvertTo(object sourceValue, Type destinationType)
		{
			return false;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000529DB File Offset: 0x00050BDB
		public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			throw PSTraceSource.NewNotSupportedException();
		}
	}
}
