using System;

namespace System.Management.Automation.Host
{
	// Token: 0x020001E9 RID: 489
	public sealed class ChoiceDescription
	{
		// Token: 0x0600165E RID: 5726 RVA: 0x0008F2AC File Offset: 0x0008D4AC
		public ChoiceDescription(string label)
		{
			if (string.IsNullOrEmpty(label))
			{
				throw PSTraceSource.NewArgumentException("label", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"label"
				});
			}
			this.label = label;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0008F2FC File Offset: 0x0008D4FC
		public ChoiceDescription(string label, string helpMessage)
		{
			if (string.IsNullOrEmpty(label))
			{
				throw PSTraceSource.NewArgumentException("label", DescriptionsStrings.NullOrEmptyErrorTemplate, new object[]
				{
					"label"
				});
			}
			if (helpMessage == null)
			{
				throw PSTraceSource.NewArgumentNullException("helpMessage");
			}
			this.label = label;
			this.helpMessage = helpMessage;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0008F35E File Offset: 0x0008D55E
		internal ChoiceDescription()
		{
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x0008F371 File Offset: 0x0008D571
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x0008F379 File Offset: 0x0008D579
		// (set) Token: 0x06001663 RID: 5731 RVA: 0x0008F381 File Offset: 0x0008D581
		public string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this.helpMessage = value;
			}
		}

		// Token: 0x04000986 RID: 2438
		private readonly string label;

		// Token: 0x04000987 RID: 2439
		private string helpMessage = "";
	}
}
