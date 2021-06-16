using System;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A72 RID: 2674
	internal class ComTypeLibMemberDesc
	{
		// Token: 0x06006A75 RID: 27253 RVA: 0x00217293 File Offset: 0x00215493
		internal ComTypeLibMemberDesc(ComType kind)
		{
			this._kind = kind;
		}

		// Token: 0x17001DB1 RID: 7601
		// (get) Token: 0x06006A76 RID: 27254 RVA: 0x002172A2 File Offset: 0x002154A2
		public ComType Kind
		{
			get
			{
				return this._kind;
			}
		}

		// Token: 0x0400330B RID: 13067
		private readonly ComType _kind;
	}
}
