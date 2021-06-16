using System;

namespace System.Management.Automation
{
	// Token: 0x0200013C RID: 316
	public class PSAdaptedProperty : PSProperty
	{
		// Token: 0x0600109A RID: 4250 RVA: 0x0005D48A File Offset: 0x0005B68A
		public PSAdaptedProperty(string name, object tag) : base(name, null, null, tag)
		{
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0005D496 File Offset: 0x0005B696
		internal PSAdaptedProperty(string name, Adapter adapter, object baseObject, object adapterData) : base(name, adapter, baseObject, adapterData)
		{
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0005D4A4 File Offset: 0x0005B6A4
		public override PSMemberInfo Copy()
		{
			PSAdaptedProperty psadaptedProperty = new PSAdaptedProperty(this.name, this.adapter, this.baseObject, this.adapterData);
			base.CloneBaseProperties(psadaptedProperty);
			psadaptedProperty.typeOfValue = this.typeOfValue;
			psadaptedProperty.serializedValue = this.serializedValue;
			psadaptedProperty.isDeserialized = this.isDeserialized;
			return psadaptedProperty;
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x0005D4FB File Offset: 0x0005B6FB
		public object BaseObject
		{
			get
			{
				return this.baseObject;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0005D503 File Offset: 0x0005B703
		public object Tag
		{
			get
			{
				return this.adapterData;
			}
		}
	}
}
