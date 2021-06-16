using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000079 RID: 121
	public class CommandParameterInfo
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x0001F1F0 File Offset: 0x0001D3F0
		internal CommandParameterInfo(CompiledCommandParameter parameter, uint parameterSetFlag)
		{
			if (parameter == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameter");
			}
			this.name = parameter.Name;
			this.parameterType = parameter.Type;
			this.isDynamic = parameter.IsDynamic;
			this.aliases = new ReadOnlyCollection<string>(parameter.Aliases);
			this.SetAttributes(parameter.CompiledAttributes);
			this.SetParameterSetData(parameter.GetParameterSetData(parameterSetFlag));
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001F280 File Offset: 0x0001D480
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001F288 File Offset: 0x0001D488
		public Type ParameterType
		{
			get
			{
				return this.parameterType;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001F290 File Offset: 0x0001D490
		public bool IsMandatory
		{
			get
			{
				return this.isMandatory;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001F298 File Offset: 0x0001D498
		public bool IsDynamic
		{
			get
			{
				return this.isDynamic;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001F2A0 File Offset: 0x0001D4A0
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001F2A8 File Offset: 0x0001D4A8
		public bool ValueFromPipeline
		{
			get
			{
				return this.valueFromPipeline;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001F2B0 File Offset: 0x0001D4B0
		public bool ValueFromPipelineByPropertyName
		{
			get
			{
				return this.valueFromPipelineByPropertyName;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
		public bool ValueFromRemainingArguments
		{
			get
			{
				return this.valueFromRemainingArguments;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		public string HelpMessage
		{
			get
			{
				return this.helpMessage;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
		public ReadOnlyCollection<string> Aliases
		{
			get
			{
				return this.aliases;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001F2D0 File Offset: 0x0001D4D0
		public ReadOnlyCollection<Attribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001F2D8 File Offset: 0x0001D4D8
		private void SetAttributes(IList<Attribute> attributeMetadata)
		{
			Collection<Attribute> collection = new Collection<Attribute>();
			foreach (Attribute item in attributeMetadata)
			{
				collection.Add(item);
			}
			this.attributes = new ReadOnlyCollection<Attribute>(collection);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001F334 File Offset: 0x0001D534
		private void SetParameterSetData(ParameterSetSpecificMetadata parameterMetadata)
		{
			this.isMandatory = parameterMetadata.IsMandatory;
			this.position = parameterMetadata.Position;
			this.valueFromPipeline = parameterMetadata.valueFromPipeline;
			this.valueFromPipelineByPropertyName = parameterMetadata.valueFromPipelineByPropertyName;
			this.valueFromRemainingArguments = parameterMetadata.ValueFromRemainingArguments;
			this.helpMessage = parameterMetadata.HelpMessage;
		}

		// Token: 0x04000294 RID: 660
		private string name = string.Empty;

		// Token: 0x04000295 RID: 661
		private Type parameterType;

		// Token: 0x04000296 RID: 662
		private bool isMandatory;

		// Token: 0x04000297 RID: 663
		private bool isDynamic;

		// Token: 0x04000298 RID: 664
		private int position = int.MinValue;

		// Token: 0x04000299 RID: 665
		private bool valueFromPipeline;

		// Token: 0x0400029A RID: 666
		private bool valueFromPipelineByPropertyName;

		// Token: 0x0400029B RID: 667
		private bool valueFromRemainingArguments;

		// Token: 0x0400029C RID: 668
		private string helpMessage = string.Empty;

		// Token: 0x0400029D RID: 669
		private ReadOnlyCollection<string> aliases;

		// Token: 0x0400029E RID: 670
		private ReadOnlyCollection<Attribute> attributes;
	}
}
