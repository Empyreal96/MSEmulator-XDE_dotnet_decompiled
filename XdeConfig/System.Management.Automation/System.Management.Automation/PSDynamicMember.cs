using System;

namespace System.Management.Automation
{
	// Token: 0x0200014A RID: 330
	public class PSDynamicMember : PSMemberInfo
	{
		// Token: 0x06001124 RID: 4388 RVA: 0x0005F2A0 File Offset: 0x0005D4A0
		internal PSDynamicMember(string name)
		{
			this.name = name;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0005F2AF File Offset: 0x0005D4AF
		public override string ToString()
		{
			return "dynamic " + base.Name;
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x0005F2C1 File Offset: 0x0005D4C1
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.Dynamic;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x0005F2C8 File Offset: 0x0005D4C8
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x0005F2CF File Offset: 0x0005D4CF
		public override object Value
		{
			get
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			set
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0005F2D6 File Offset: 0x0005D4D6
		public override string TypeNameOfValue
		{
			get
			{
				return "dynamic";
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0005F2DD File Offset: 0x0005D4DD
		public override PSMemberInfo Copy()
		{
			return new PSDynamicMember(base.Name);
		}
	}
}
