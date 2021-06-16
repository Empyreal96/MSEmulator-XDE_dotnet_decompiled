using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200041A RID: 1050
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class ParameterAttribute : ParsingBaseAttribute
	{
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x00100440 File Offset: 0x000FE640
		// (set) Token: 0x06002EBF RID: 11967 RVA: 0x00100448 File Offset: 0x000FE648
		public int Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x00100451 File Offset: 0x000FE651
		// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x00100459 File Offset: 0x000FE659
		public string ParameterSetName
		{
			get
			{
				return this.parameterSetName;
			}
			set
			{
				this.parameterSetName = value;
				if (string.IsNullOrEmpty(this.parameterSetName))
				{
					this.parameterSetName = "__AllParameterSets";
				}
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0010047A File Offset: 0x000FE67A
		// (set) Token: 0x06002EC3 RID: 11971 RVA: 0x00100482 File Offset: 0x000FE682
		public bool Mandatory
		{
			get
			{
				return this.mandatory;
			}
			set
			{
				this.mandatory = value;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x0010048B File Offset: 0x000FE68B
		// (set) Token: 0x06002EC5 RID: 11973 RVA: 0x00100493 File Offset: 0x000FE693
		public bool ValueFromPipeline
		{
			get
			{
				return this.valueFromPipeline;
			}
			set
			{
				this.valueFromPipeline = value;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0010049C File Offset: 0x000FE69C
		// (set) Token: 0x06002EC7 RID: 11975 RVA: 0x001004A4 File Offset: 0x000FE6A4
		public bool ValueFromPipelineByPropertyName
		{
			get
			{
				return this.valueFromPipelineByPropertyName;
			}
			set
			{
				this.valueFromPipelineByPropertyName = value;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x001004AD File Offset: 0x000FE6AD
		// (set) Token: 0x06002EC9 RID: 11977 RVA: 0x001004B5 File Offset: 0x000FE6B5
		public bool ValueFromRemainingArguments
		{
			get
			{
				return this.valueFromRemainingArguments;
			}
			set
			{
				this.valueFromRemainingArguments = value;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x001004BE File Offset: 0x000FE6BE
		// (set) Token: 0x06002ECB RID: 11979 RVA: 0x001004C6 File Offset: 0x000FE6C6
		public string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("value");
				}
				this.helpMessage = value;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x001004E2 File Offset: 0x000FE6E2
		// (set) Token: 0x06002ECD RID: 11981 RVA: 0x001004EA File Offset: 0x000FE6EA
		public string HelpMessageBaseName
		{
			get
			{
				return this.helpMessageBaseName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("value");
				}
				this.helpMessageBaseName = value;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x00100506 File Offset: 0x000FE706
		// (set) Token: 0x06002ECF RID: 11983 RVA: 0x0010050E File Offset: 0x000FE70E
		public string HelpMessageResourceId
		{
			get
			{
				return this.helpMessageResourceId;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("value");
				}
				this.helpMessageResourceId = value;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x0010052A File Offset: 0x000FE72A
		// (set) Token: 0x06002ED1 RID: 11985 RVA: 0x00100532 File Offset: 0x000FE732
		public bool DontShow { get; set; }

		// Token: 0x04001895 RID: 6293
		public const string AllParameterSets = "__AllParameterSets";

		// Token: 0x04001896 RID: 6294
		private int position = int.MinValue;

		// Token: 0x04001897 RID: 6295
		private string parameterSetName = "__AllParameterSets";

		// Token: 0x04001898 RID: 6296
		private bool mandatory;

		// Token: 0x04001899 RID: 6297
		private bool valueFromRemainingArguments;

		// Token: 0x0400189A RID: 6298
		private string helpMessage;

		// Token: 0x0400189B RID: 6299
		private string helpMessageBaseName;

		// Token: 0x0400189C RID: 6300
		private string helpMessageResourceId;

		// Token: 0x0400189D RID: 6301
		private bool valueFromPipeline;

		// Token: 0x0400189E RID: 6302
		private bool valueFromPipelineByPropertyName;
	}
}
