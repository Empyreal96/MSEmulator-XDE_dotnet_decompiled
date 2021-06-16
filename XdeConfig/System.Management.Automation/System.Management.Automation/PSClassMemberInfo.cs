using System;

namespace System.Management.Automation
{
	// Token: 0x0200009F RID: 159
	public sealed class PSClassMemberInfo
	{
		// Token: 0x060007A7 RID: 1959 RVA: 0x00025CBB File Offset: 0x00023EBB
		internal PSClassMemberInfo(string name, string memberType, string defaultValue)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this.TypeName = memberType;
			this.DefaultValue = defaultValue;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00025CEB File Offset: 0x00023EEB
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00025CF3 File Offset: 0x00023EF3
		public string Name { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00025CFC File Offset: 0x00023EFC
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x00025D04 File Offset: 0x00023F04
		public string TypeName { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00025D0D File Offset: 0x00023F0D
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x00025D15 File Offset: 0x00023F15
		public string DefaultValue { get; private set; }
	}
}
