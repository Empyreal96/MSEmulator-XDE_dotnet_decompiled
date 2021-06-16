using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200098E RID: 2446
	[Cmdlet("Register", "ArgumentCompleter", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=528576")]
	public class RegisterArgumentCompleterCommand : PSCmdlet
	{
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x001E5E55 File Offset: 0x001E4055
		// (set) Token: 0x06005A5F RID: 23135 RVA: 0x001E5E5D File Offset: 0x001E405D
		[Parameter(ParameterSetName = "PowerShellSet")]
		[Parameter(ParameterSetName = "NativeSet", Mandatory = true)]
		public string[] CommandName { get; set; }

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06005A60 RID: 23136 RVA: 0x001E5E66 File Offset: 0x001E4066
		// (set) Token: 0x06005A61 RID: 23137 RVA: 0x001E5E6E File Offset: 0x001E406E
		[Parameter(ParameterSetName = "PowerShellSet", Mandatory = true)]
		public string ParameterName { get; set; }

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06005A62 RID: 23138 RVA: 0x001E5E77 File Offset: 0x001E4077
		// (set) Token: 0x06005A63 RID: 23139 RVA: 0x001E5E7F File Offset: 0x001E407F
		[AllowNull]
		[Parameter(Mandatory = true)]
		public ScriptBlock ScriptBlock { get; set; }

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x001E5E88 File Offset: 0x001E4088
		// (set) Token: 0x06005A65 RID: 23141 RVA: 0x001E5E90 File Offset: 0x001E4090
		[Parameter(ParameterSetName = "NativeSet")]
		public SwitchParameter Native { get; set; }

		// Token: 0x06005A66 RID: 23142 RVA: 0x001E5E9C File Offset: 0x001E409C
		protected override void EndProcessing()
		{
			Dictionary<string, ScriptBlock> dictionary2;
			if (this.ParameterName != null)
			{
				Dictionary<string, ScriptBlock> dictionary;
				if ((dictionary = base.Context.CustomArgumentCompleters) == null)
				{
					dictionary = (base.Context.CustomArgumentCompleters = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase));
				}
				dictionary2 = dictionary;
			}
			else
			{
				Dictionary<string, ScriptBlock> dictionary3;
				if ((dictionary3 = base.Context.NativeArgumentCompleters) == null)
				{
					dictionary3 = (base.Context.NativeArgumentCompleters = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase));
				}
				dictionary2 = dictionary3;
			}
			if (this.CommandName == null || this.CommandName.Length == 0)
			{
				this.CommandName = new string[]
				{
					""
				};
			}
			for (int i = 0; i < this.CommandName.Length; i++)
			{
				string text = this.CommandName[i];
				if (!string.IsNullOrWhiteSpace(this.ParameterName))
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						text = text + ":" + this.ParameterName;
					}
					else
					{
						text = this.ParameterName;
					}
				}
				dictionary2[text] = this.ScriptBlock;
			}
		}
	}
}
