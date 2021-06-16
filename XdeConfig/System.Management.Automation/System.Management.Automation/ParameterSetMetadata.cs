using System;
using System.Globalization;
using System.Management.Automation.Language;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000086 RID: 134
	public sealed class ParameterSetMetadata
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x00020B25 File Offset: 0x0001ED25
		internal ParameterSetMetadata(ParameterSetSpecificMetadata psMD)
		{
			this.Initialize(psMD);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00020B34 File Offset: 0x0001ED34
		internal ParameterSetMetadata(ParameterSetMetadata other)
		{
			if (other == null)
			{
				throw PSTraceSource.NewArgumentNullException("other");
			}
			this.helpMessage = other.helpMessage;
			this.helpMessageBaseName = other.helpMessageBaseName;
			this.helpMessageResourceId = other.helpMessageResourceId;
			this.isMandatory = other.isMandatory;
			this.position = other.position;
			this.valueFromPipeline = other.valueFromPipeline;
			this.valueFromPipelineByPropertyName = other.valueFromPipelineByPropertyName;
			this.valueFromRemainingArguments = other.valueFromRemainingArguments;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00020BB5 File Offset: 0x0001EDB5
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00020BBD File Offset: 0x0001EDBD
		public bool IsMandatory
		{
			get
			{
				return this.isMandatory;
			}
			set
			{
				this.isMandatory = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00020BC6 File Offset: 0x0001EDC6
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00020BCE File Offset: 0x0001EDCE
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

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00020BD7 File Offset: 0x0001EDD7
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00020BDF File Offset: 0x0001EDDF
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00020BE8 File Offset: 0x0001EDE8
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00020BF0 File Offset: 0x0001EDF0
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00020BF9 File Offset: 0x0001EDF9
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00020C01 File Offset: 0x0001EE01
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00020C0A File Offset: 0x0001EE0A
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00020C12 File Offset: 0x0001EE12
		public string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
			set
			{
				this.helpMessage = value;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00020C1B File Offset: 0x0001EE1B
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00020C23 File Offset: 0x0001EE23
		public string HelpMessageBaseName
		{
			get
			{
				return this.helpMessageBaseName;
			}
			set
			{
				this.helpMessageBaseName = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00020C2C File Offset: 0x0001EE2C
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x00020C34 File Offset: 0x0001EE34
		public string HelpMessageResourceId
		{
			get
			{
				return this.helpMessageResourceId;
			}
			set
			{
				this.helpMessageResourceId = value;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00020C40 File Offset: 0x0001EE40
		internal void Initialize(ParameterSetSpecificMetadata psMD)
		{
			this.isMandatory = psMD.IsMandatory;
			this.position = psMD.Position;
			this.valueFromPipeline = psMD.ValueFromPipeline;
			this.valueFromPipelineByPropertyName = psMD.ValueFromPipelineByPropertyName;
			this.valueFromRemainingArguments = psMD.ValueFromRemainingArguments;
			this.helpMessage = psMD.HelpMessage;
			this.helpMessageBaseName = psMD.HelpMessageBaseName;
			this.helpMessageResourceId = psMD.HelpMessageResourceId;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00020CB0 File Offset: 0x0001EEB0
		internal bool Equals(ParameterSetMetadata second)
		{
			return this.isMandatory == second.isMandatory && this.position == second.position && this.valueFromPipeline == second.valueFromPipeline && this.valueFromPipelineByPropertyName == second.valueFromPipelineByPropertyName && this.valueFromRemainingArguments == second.valueFromRemainingArguments && !(this.helpMessage != second.helpMessage) && !(this.helpMessageBaseName != second.helpMessageBaseName) && !(this.helpMessageResourceId != second.helpMessageResourceId);
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00020D40 File Offset: 0x0001EF40
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00020D80 File Offset: 0x0001EF80
		internal ParameterSetMetadata.ParameterFlags Flags
		{
			get
			{
				ParameterSetMetadata.ParameterFlags parameterFlags = (ParameterSetMetadata.ParameterFlags)0U;
				if (this.IsMandatory)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.Mandatory;
				}
				if (this.ValueFromPipeline)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.ValueFromPipeline;
				}
				if (this.ValueFromPipelineByPropertyName)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName;
				}
				if (this.ValueFromRemainingArguments)
				{
					parameterFlags |= ParameterSetMetadata.ParameterFlags.ValueFromRemainingArguments;
				}
				return parameterFlags;
			}
			set
			{
				this.IsMandatory = (ParameterSetMetadata.ParameterFlags.Mandatory == (value & ParameterSetMetadata.ParameterFlags.Mandatory));
				this.ValueFromPipeline = (ParameterSetMetadata.ParameterFlags.ValueFromPipeline == (value & ParameterSetMetadata.ParameterFlags.ValueFromPipeline));
				this.ValueFromPipelineByPropertyName = (ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName == (value & ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName));
				this.ValueFromRemainingArguments = (ParameterSetMetadata.ParameterFlags.ValueFromRemainingArguments == (value & ParameterSetMetadata.ParameterFlags.ValueFromRemainingArguments));
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00020DB2 File Offset: 0x0001EFB2
		internal ParameterSetMetadata(int position, ParameterSetMetadata.ParameterFlags flags, string helpMessage)
		{
			this.Position = position;
			this.Flags = flags;
			this.HelpMessage = helpMessage;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00020DD0 File Offset: 0x0001EFD0
		internal string GetProxyParameterData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			if (this.isMandatory)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}Mandatory=$true", new object[]
				{
					text
				});
				text = ", ";
			}
			if (this.position != -2147483648)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}Position={1}", new object[]
				{
					text,
					this.position
				});
				text = ", ";
			}
			if (this.valueFromPipeline)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}ValueFromPipeline=$true", new object[]
				{
					text
				});
				text = ", ";
			}
			if (this.valueFromPipelineByPropertyName)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}ValueFromPipelineByPropertyName=$true", new object[]
				{
					text
				});
				text = ", ";
			}
			if (this.valueFromRemainingArguments)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}ValueFromRemainingArguments=$true", new object[]
				{
					text
				});
				text = ", ";
			}
			if (!string.IsNullOrEmpty(this.helpMessage))
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}HelpMessage='{1}'", new object[]
				{
					text,
					CodeGeneration.EscapeSingleQuotedStringContent(this.helpMessage)
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040002C9 RID: 713
		private const string MandatoryFormat = "{0}Mandatory=$true";

		// Token: 0x040002CA RID: 714
		private const string PositionFormat = "{0}Position={1}";

		// Token: 0x040002CB RID: 715
		private const string ValueFromPipelineFormat = "{0}ValueFromPipeline=$true";

		// Token: 0x040002CC RID: 716
		private const string ValueFromPipelineByPropertyNameFormat = "{0}ValueFromPipelineByPropertyName=$true";

		// Token: 0x040002CD RID: 717
		private const string ValueFromRemainingArgumentsFormat = "{0}ValueFromRemainingArguments=$true";

		// Token: 0x040002CE RID: 718
		private const string HelpMessageFormat = "{0}HelpMessage='{1}'";

		// Token: 0x040002CF RID: 719
		private bool isMandatory;

		// Token: 0x040002D0 RID: 720
		private int position;

		// Token: 0x040002D1 RID: 721
		private bool valueFromPipeline;

		// Token: 0x040002D2 RID: 722
		private bool valueFromPipelineByPropertyName;

		// Token: 0x040002D3 RID: 723
		private bool valueFromRemainingArguments;

		// Token: 0x040002D4 RID: 724
		private string helpMessage;

		// Token: 0x040002D5 RID: 725
		private string helpMessageBaseName;

		// Token: 0x040002D6 RID: 726
		private string helpMessageResourceId;

		// Token: 0x02000087 RID: 135
		[Flags]
		internal enum ParameterFlags : uint
		{
			// Token: 0x040002D8 RID: 728
			Mandatory = 1U,
			// Token: 0x040002D9 RID: 729
			ValueFromPipeline = 2U,
			// Token: 0x040002DA RID: 730
			ValueFromPipelineByPropertyName = 4U,
			// Token: 0x040002DB RID: 731
			ValueFromRemainingArguments = 8U
		}
	}
}
