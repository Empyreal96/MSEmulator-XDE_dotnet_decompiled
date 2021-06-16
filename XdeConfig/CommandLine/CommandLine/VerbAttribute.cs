using System;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x02000051 RID: 81
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class VerbAttribute : Attribute
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x00008401 File Offset: 0x00006601
		public VerbAttribute(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("name");
			}
			this.name = name;
			this.helpText = new LocalizableAttributeProperty("HelpText");
			this.resourceType = null;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000843A File Offset: 0x0000663A
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008442 File Offset: 0x00006642
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000844A File Offset: 0x0000664A
		public bool Hidden { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008453 File Offset: 0x00006653
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008469 File Offset: 0x00006669
		public string HelpText
		{
			get
			{
				return this.helpText.Value ?? string.Empty;
			}
			set
			{
				LocalizableAttributeProperty localizableAttributeProperty = this.helpText;
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				localizableAttributeProperty.Value = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008486 File Offset: 0x00006686
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00008490 File Offset: 0x00006690
		public Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
			set
			{
				this.helpText.ResourceType = value;
				this.resourceType = value;
			}
		}

		// Token: 0x0400008C RID: 140
		private readonly string name;

		// Token: 0x0400008D RID: 141
		private LocalizableAttributeProperty helpText;

		// Token: 0x0400008E RID: 142
		private Type resourceType;
	}
}
