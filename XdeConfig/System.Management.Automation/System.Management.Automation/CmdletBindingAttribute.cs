using System;

namespace System.Management.Automation
{
	// Token: 0x02000416 RID: 1046
	[AttributeUsage(AttributeTargets.Class)]
	public class CmdletBindingAttribute : CmdletCommonMetadataAttribute
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x001002E4 File Offset: 0x000FE4E4
		// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x001002EC File Offset: 0x000FE4EC
		public bool PositionalBinding
		{
			get
			{
				return this._positionalBinding;
			}
			set
			{
				this._positionalBinding = value;
			}
		}

		// Token: 0x04001890 RID: 6288
		private bool _positionalBinding = true;
	}
}
