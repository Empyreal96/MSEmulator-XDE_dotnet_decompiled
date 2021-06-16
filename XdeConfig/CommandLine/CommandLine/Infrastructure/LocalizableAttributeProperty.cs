using System;
using System.Reflection;

namespace CommandLine.Infrastructure
{
	// Token: 0x02000060 RID: 96
	internal class LocalizableAttributeProperty
	{
		// Token: 0x06000273 RID: 627 RVA: 0x0000A22C File Offset: 0x0000842C
		public LocalizableAttributeProperty(string propertyName)
		{
			this._propertyName = propertyName;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000A23B File Offset: 0x0000843B
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000A243 File Offset: 0x00008443
		public string Value
		{
			get
			{
				return this.GetLocalizedValue();
			}
			set
			{
				this._localizationPropertyInfo = null;
				this._value = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000A253 File Offset: 0x00008453
		public Type ResourceType
		{
			set
			{
				this._localizationPropertyInfo = null;
				this._type = value;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A264 File Offset: 0x00008464
		private string GetLocalizedValue()
		{
			if (string.IsNullOrEmpty(this._value) || this._type == null)
			{
				return this._value;
			}
			if (this._localizationPropertyInfo == null)
			{
				if (!this._type.IsVisible)
				{
					throw new ArgumentException("Invalid resource type", this._propertyName);
				}
				PropertyInfo property = this._type.GetProperty(this._value, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
				if (property == null || !property.CanRead || property.PropertyType != typeof(string))
				{
					throw new ArgumentException("Invalid resource property name", this._propertyName);
				}
				this._localizationPropertyInfo = property;
			}
			return (string)this._localizationPropertyInfo.GetValue(null, null);
		}

		// Token: 0x040000BE RID: 190
		private string _propertyName;

		// Token: 0x040000BF RID: 191
		private string _value;

		// Token: 0x040000C0 RID: 192
		private Type _type;

		// Token: 0x040000C1 RID: 193
		private PropertyInfo _localizationPropertyInfo;
	}
}
