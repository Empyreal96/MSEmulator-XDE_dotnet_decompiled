using System;
using System.Globalization;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200003F RID: 63
	public sealed class PagingParameters
	{
		// Token: 0x06000312 RID: 786 RVA: 0x0000BA9D File Offset: 0x00009C9D
		internal PagingParameters(MshCommandRuntime commandRuntime)
		{
			if (commandRuntime == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandRuntime");
			}
			commandRuntime.PagingParameters = this;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000BAC2 File Offset: 0x00009CC2
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000BACA File Offset: 0x00009CCA
		[Parameter]
		public SwitchParameter IncludeTotalCount { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000BAD3 File Offset: 0x00009CD3
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000BADB File Offset: 0x00009CDB
		[Parameter]
		public ulong Skip { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000BAEC File Offset: 0x00009CEC
		[Parameter]
		public ulong First
		{
			get
			{
				return this.psFirst;
			}
			set
			{
				this.psFirst = value;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		public PSObject NewTotalCount(ulong totalCount, double accuracy)
		{
			PSObject psobject = new PSObject(totalCount);
			string script = string.Format(CultureInfo.CurrentCulture, "\r\n                    $totalCount = $this.PSObject.BaseObject\r\n                    switch ($this.Accuracy) {{\r\n                        {{ $_ -ge 1.0 }} {{ '{0}' -f $totalCount }}\r\n                        {{ $_ -le 0.0 }} {{ '{1}' -f $totalCount }}\r\n                        default          {{ '{2}' -f $totalCount }}\r\n                    }}\r\n                ", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(CommandBaseStrings.PagingSupportAccurateTotalCountTemplate),
				CodeGeneration.EscapeSingleQuotedStringContent(CommandBaseStrings.PagingSupportUnknownTotalCountTemplate),
				CodeGeneration.EscapeSingleQuotedStringContent(CommandBaseStrings.PagingSupportEstimatedTotalCountTemplate)
			});
			PSScriptMethod member = new PSScriptMethod("ToString", ScriptBlock.Create(script));
			psobject.Members.Add(member);
			accuracy = Math.Max(0.0, Math.Min(1.0, accuracy));
			PSNoteProperty member2 = new PSNoteProperty("Accuracy", accuracy);
			psobject.Members.Add(member2);
			return psobject;
		}

		// Token: 0x04000106 RID: 262
		private ulong psFirst = ulong.MaxValue;
	}
}
