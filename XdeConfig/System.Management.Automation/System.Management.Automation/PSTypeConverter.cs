using System;

namespace System.Management.Automation
{
	// Token: 0x02000116 RID: 278
	public abstract class PSTypeConverter
	{
		// Token: 0x06000EF2 RID: 3826 RVA: 0x00052938 File Offset: 0x00050B38
		private static object GetSourceValueAsObject(PSObject sourceValue)
		{
			if (sourceValue == null)
			{
				return null;
			}
			if (sourceValue.BaseObject is PSCustomObject)
			{
				return sourceValue;
			}
			return PSObject.Base(sourceValue);
		}

		// Token: 0x06000EF3 RID: 3827
		public abstract bool CanConvertFrom(object sourceValue, Type destinationType);

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00052954 File Offset: 0x00050B54
		public virtual bool CanConvertFrom(PSObject sourceValue, Type destinationType)
		{
			return this.CanConvertFrom(PSTypeConverter.GetSourceValueAsObject(sourceValue), destinationType);
		}

		// Token: 0x06000EF5 RID: 3829
		public abstract object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase);

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00052963 File Offset: 0x00050B63
		public virtual object ConvertFrom(PSObject sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			return this.ConvertFrom(PSTypeConverter.GetSourceValueAsObject(sourceValue), destinationType, formatProvider, ignoreCase);
		}

		// Token: 0x06000EF7 RID: 3831
		public abstract bool CanConvertTo(object sourceValue, Type destinationType);

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00052975 File Offset: 0x00050B75
		public virtual bool CanConvertTo(PSObject sourceValue, Type destinationType)
		{
			return this.CanConvertTo(PSTypeConverter.GetSourceValueAsObject(sourceValue), destinationType);
		}

		// Token: 0x06000EF9 RID: 3833
		public abstract object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase);

		// Token: 0x06000EFA RID: 3834 RVA: 0x00052984 File Offset: 0x00050B84
		public virtual object ConvertTo(PSObject sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			return this.ConvertTo(PSTypeConverter.GetSourceValueAsObject(sourceValue), destinationType, formatProvider, ignoreCase);
		}
	}
}
