using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000133 RID: 307
	public class NullString
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x0005C787 File Offset: 0x0005A987
		public override string ToString()
		{
			return null;
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001050 RID: 4176 RVA: 0x0005C78A File Offset: 0x0005A98A
		public static NullString Value
		{
			get
			{
				return NullString._value;
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0005C791 File Offset: 0x0005A991
		private NullString()
		{
		}

		// Token: 0x0400070D RID: 1805
		private static readonly NullString _value = new NullString();
	}
}
