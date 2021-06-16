using System;
using System.IO;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000772 RID: 1906
	internal sealed class GetChildDynamicParameters
	{
		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00194520 File Offset: 0x00192720
		// (set) Token: 0x06004C4D RID: 19533 RVA: 0x00194528 File Offset: 0x00192728
		[Parameter]
		public FlagsExpression<FileAttributes> Attributes
		{
			get
			{
				return this.evaluator;
			}
			set
			{
				this.evaluator = value;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x00194531 File Offset: 0x00192731
		// (set) Token: 0x06004C4F RID: 19535 RVA: 0x0019453E File Offset: 0x0019273E
		[Alias(new string[]
		{
			"ad",
			"d"
		})]
		[Parameter]
		public SwitchParameter Directory
		{
			get
			{
				return this.attributeDirectory;
			}
			set
			{
				this.attributeDirectory = value;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x0019454C File Offset: 0x0019274C
		// (set) Token: 0x06004C51 RID: 19537 RVA: 0x00194559 File Offset: 0x00192759
		[Parameter]
		[Alias(new string[]
		{
			"af"
		})]
		public SwitchParameter File
		{
			get
			{
				return this.attributeFile;
			}
			set
			{
				this.attributeFile = value;
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x00194567 File Offset: 0x00192767
		// (set) Token: 0x06004C53 RID: 19539 RVA: 0x00194574 File Offset: 0x00192774
		[Alias(new string[]
		{
			"ah",
			"h"
		})]
		[Parameter]
		public SwitchParameter Hidden
		{
			get
			{
				return this.attributeHidden;
			}
			set
			{
				this.attributeHidden = value;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x00194582 File Offset: 0x00192782
		// (set) Token: 0x06004C55 RID: 19541 RVA: 0x0019458F File Offset: 0x0019278F
		[Alias(new string[]
		{
			"ar"
		})]
		[Parameter]
		public SwitchParameter ReadOnly
		{
			get
			{
				return this.attributeReadOnly;
			}
			set
			{
				this.attributeReadOnly = value;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06004C56 RID: 19542 RVA: 0x0019459D File Offset: 0x0019279D
		// (set) Token: 0x06004C57 RID: 19543 RVA: 0x001945AA File Offset: 0x001927AA
		[Alias(new string[]
		{
			"as"
		})]
		[Parameter]
		public SwitchParameter System
		{
			get
			{
				return this.attributeSystem;
			}
			set
			{
				this.attributeSystem = value;
			}
		}

		// Token: 0x040024CE RID: 9422
		private FlagsExpression<FileAttributes> evaluator;

		// Token: 0x040024CF RID: 9423
		private bool attributeDirectory;

		// Token: 0x040024D0 RID: 9424
		private bool attributeFile;

		// Token: 0x040024D1 RID: 9425
		private bool attributeHidden;

		// Token: 0x040024D2 RID: 9426
		private bool attributeReadOnly;

		// Token: 0x040024D3 RID: 9427
		private bool attributeSystem;
	}
}
