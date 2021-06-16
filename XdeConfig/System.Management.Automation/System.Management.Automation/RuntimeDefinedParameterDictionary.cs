using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000080 RID: 128
	[Serializable]
	public class RuntimeDefinedParameterDictionary : Dictionary<string, RuntimeDefinedParameter>
	{
		// Token: 0x0600069C RID: 1692 RVA: 0x0001FF2A File Offset: 0x0001E12A
		public RuntimeDefinedParameterDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001FF42 File Offset: 0x0001E142
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0001FF4A File Offset: 0x0001E14A
		public string HelpFile
		{
			get
			{
				return this._helpFile;
			}
			set
			{
				this._helpFile = (string.IsNullOrEmpty(value) ? string.Empty : value);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001FF62 File Offset: 0x0001E162
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001FF6A File Offset: 0x0001E16A
		public object Data { get; set; }

		// Token: 0x040002BA RID: 698
		private string _helpFile = string.Empty;

		// Token: 0x040002BB RID: 699
		internal static RuntimeDefinedParameter[] EmptyParameterArray = new RuntimeDefinedParameter[0];
	}
}
