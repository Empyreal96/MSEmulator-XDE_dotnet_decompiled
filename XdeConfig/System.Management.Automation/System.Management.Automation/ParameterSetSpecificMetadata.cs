using System;

namespace System.Management.Automation
{
	// Token: 0x0200007C RID: 124
	internal class ParameterSetSpecificMetadata
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x0001FAA4 File Offset: 0x0001DCA4
		internal ParameterSetSpecificMetadata(ParameterAttribute attribute)
		{
			if (attribute == null)
			{
				throw PSTraceSource.NewArgumentNullException("attribute");
			}
			this.attribute = attribute;
			this.isMandatory = attribute.Mandatory;
			this.position = attribute.Position;
			this.valueFromRemainingArguments = attribute.ValueFromRemainingArguments;
			this.valueFromPipeline = attribute.ValueFromPipeline;
			this.valueFromPipelineByPropertyName = attribute.ValueFromPipelineByPropertyName;
			this.helpMessage = attribute.HelpMessage;
			this.helpMessageBaseName = attribute.HelpMessageBaseName;
			this.helpMessageResourceId = attribute.HelpMessageResourceId;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001FB38 File Offset: 0x0001DD38
		internal ParameterSetSpecificMetadata(bool isMandatory, int position, bool valueFromRemainingArguments, bool valueFromPipeline, bool valueFromPipelineByPropertyName, string helpMessageBaseName, string helpMessageResourceId, string helpMessage)
		{
			this.isMandatory = isMandatory;
			this.position = position;
			this.valueFromRemainingArguments = valueFromRemainingArguments;
			this.valueFromPipeline = valueFromPipeline;
			this.valueFromPipelineByPropertyName = valueFromPipelineByPropertyName;
			this.helpMessageBaseName = helpMessageBaseName;
			this.helpMessageResourceId = helpMessageResourceId;
			this.helpMessage = helpMessage;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001FB93 File Offset: 0x0001DD93
		internal bool IsMandatory
		{
			get
			{
				return this.isMandatory;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001FB9B File Offset: 0x0001DD9B
		internal int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001FBA3 File Offset: 0x0001DDA3
		internal bool IsPositional
		{
			get
			{
				return this.position != int.MinValue;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001FBB5 File Offset: 0x0001DDB5
		internal bool ValueFromRemainingArguments
		{
			get
			{
				return this.valueFromRemainingArguments;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001FBBD File Offset: 0x0001DDBD
		internal bool ValueFromPipeline
		{
			get
			{
				return this.valueFromPipeline;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001FBC5 File Offset: 0x0001DDC5
		internal bool ValueFromPipelineByPropertyName
		{
			get
			{
				return this.valueFromPipelineByPropertyName;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001FBCD File Offset: 0x0001DDCD
		internal string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001FBD5 File Offset: 0x0001DDD5
		internal string HelpMessageBaseName
		{
			get
			{
				return this.helpMessageBaseName;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001FBDD File Offset: 0x0001DDDD
		internal string HelpMessageResourceId
		{
			get
			{
				return this.helpMessageResourceId;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x0001FBED File Offset: 0x0001DDED
		internal bool IsInAllSets
		{
			get
			{
				return this.isInAllSets;
			}
			set
			{
				this.isInAllSets = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x0001FBF6 File Offset: 0x0001DDF6
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x0001FBFE File Offset: 0x0001DDFE
		internal uint ParameterSetFlag
		{
			get
			{
				return this.parameterSetFlag;
			}
			set
			{
				this.parameterSetFlag = value;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001FC08 File Offset: 0x0001DE08
		internal string GetHelpMessage(Cmdlet cmdlet)
		{
			string result = null;
			bool flag = !string.IsNullOrEmpty(this.HelpMessage);
			bool flag2 = !string.IsNullOrEmpty(this.HelpMessageBaseName);
			bool flag3 = !string.IsNullOrEmpty(this.HelpMessageResourceId);
			if (flag2 ^ flag3)
			{
				throw PSTraceSource.NewArgumentException(flag2 ? "HelpMessageResourceId" : "HelpMessageBaseName");
			}
			if (flag2 && flag3)
			{
				try
				{
					return cmdlet.GetResourceString(this.HelpMessageBaseName, this.HelpMessageResourceId);
				}
				catch (ArgumentException)
				{
					if (flag)
					{
						return this.HelpMessage;
					}
					throw;
				}
				catch (InvalidOperationException)
				{
					if (flag)
					{
						return this.HelpMessage;
					}
					throw;
				}
			}
			if (flag)
			{
				result = this.HelpMessage;
			}
			return result;
		}

		// Token: 0x040002A8 RID: 680
		private bool isMandatory;

		// Token: 0x040002A9 RID: 681
		private int position = int.MinValue;

		// Token: 0x040002AA RID: 682
		private bool valueFromRemainingArguments;

		// Token: 0x040002AB RID: 683
		internal bool valueFromPipeline;

		// Token: 0x040002AC RID: 684
		internal bool valueFromPipelineByPropertyName;

		// Token: 0x040002AD RID: 685
		private string helpMessage;

		// Token: 0x040002AE RID: 686
		private string helpMessageBaseName;

		// Token: 0x040002AF RID: 687
		private string helpMessageResourceId;

		// Token: 0x040002B0 RID: 688
		private bool isInAllSets;

		// Token: 0x040002B1 RID: 689
		private uint parameterSetFlag;

		// Token: 0x040002B2 RID: 690
		private ParameterAttribute attribute;
	}
}
