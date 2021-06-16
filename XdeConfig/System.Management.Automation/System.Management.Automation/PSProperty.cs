using System;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200013B RID: 315
	public class PSProperty : PSPropertyInfo
	{
		// Token: 0x0600108E RID: 4238 RVA: 0x0005D29C File Offset: 0x0005B49C
		public override string ToString()
		{
			if (this.isDeserialized)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.TypeNameOfValue);
				stringBuilder.Append(" {get;set;}");
				return stringBuilder.ToString();
			}
			return this.adapter.BasePropertyToString(this);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0005D2E3 File Offset: 0x0005B4E3
		internal PSProperty(string name, object serializedValue)
		{
			this.isDeserialized = true;
			this.serializedValue = serializedValue;
			this.name = name;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0005D300 File Offset: 0x0005B500
		internal PSProperty(string name, Adapter adapter, object baseObject, object adapterData)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.adapter = adapter;
			this.adapterData = adapterData;
			this.baseObject = baseObject;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0005D338 File Offset: 0x0005B538
		public override PSMemberInfo Copy()
		{
			PSProperty psproperty = new PSProperty(this.name, this.adapter, this.baseObject, this.adapterData);
			base.CloneBaseProperties(psproperty);
			psproperty.typeOfValue = this.typeOfValue;
			psproperty.serializedValue = this.serializedValue;
			psproperty.isDeserialized = this.isDeserialized;
			return psproperty;
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0005D38F File Offset: 0x0005B58F
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.Property;
			}
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0005D394 File Offset: 0x0005B594
		private object GetAdaptedValue()
		{
			if (this.isDeserialized)
			{
				return this.serializedValue;
			}
			return this.adapter.BasePropertyGet(this);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0005D3BE File Offset: 0x0005B5BE
		internal void SetAdaptedValue(object setValue, bool shouldConvert)
		{
			if (this.isDeserialized)
			{
				this.serializedValue = setValue;
				return;
			}
			this.adapter.BasePropertySet(this, setValue, shouldConvert);
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0005D3DE File Offset: 0x0005B5DE
		// (set) Token: 0x06001096 RID: 4246 RVA: 0x0005D3E6 File Offset: 0x0005B5E6
		public override object Value
		{
			get
			{
				return this.GetAdaptedValue();
			}
			set
			{
				this.SetAdaptedValue(value, true);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x0005D3F0 File Offset: 0x0005B5F0
		public override bool IsSettable
		{
			get
			{
				return this.isDeserialized || this.adapter.BasePropertyIsSettable(this);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0005D408 File Offset: 0x0005B608
		public override bool IsGettable
		{
			get
			{
				return this.isDeserialized || this.adapter.BasePropertyIsGettable(this);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0005D420 File Offset: 0x0005B620
		public override string TypeNameOfValue
		{
			get
			{
				if (!this.isDeserialized)
				{
					return this.adapter.BasePropertyType(this);
				}
				if (this.serializedValue == null)
				{
					return string.Empty;
				}
				PSObject psobject = this.serializedValue as PSObject;
				if (psobject != null)
				{
					ConsolidatedString internalTypeNames = psobject.InternalTypeNames;
					if (internalTypeNames != null && internalTypeNames.Count >= 1)
					{
						return internalTypeNames[0];
					}
				}
				return this.serializedValue.GetType().FullName;
			}
		}

		// Token: 0x04000732 RID: 1842
		internal string typeOfValue;

		// Token: 0x04000733 RID: 1843
		internal object serializedValue;

		// Token: 0x04000734 RID: 1844
		internal bool isDeserialized;

		// Token: 0x04000735 RID: 1845
		internal Adapter adapter;

		// Token: 0x04000736 RID: 1846
		internal object adapterData;

		// Token: 0x04000737 RID: 1847
		internal object baseObject;
	}
}
