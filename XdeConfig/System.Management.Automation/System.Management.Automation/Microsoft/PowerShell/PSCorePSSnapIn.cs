using System;
using System.ComponentModel;
using System.Management.Automation;

namespace Microsoft.PowerShell
{
	// Token: 0x0200085D RID: 2141
	[RunInstaller(true)]
	public sealed class PSCorePSSnapIn : PSSnapIn
	{
		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x001B7C83 File Offset: 0x001B5E83
		public override string Name
		{
			get
			{
				return "Microsoft.PowerShell.Core";
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x001B7C8A File Offset: 0x001B5E8A
		public override string Vendor
		{
			get
			{
				return "Microsoft";
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x001B7C91 File Offset: 0x001B5E91
		public override string VendorResource
		{
			get
			{
				return "CoreMshSnapInResources,Vendor";
			}
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06005263 RID: 21091 RVA: 0x001B7C98 File Offset: 0x001B5E98
		public override string Description
		{
			get
			{
				return "This PSSnapIn contains MSH management cmdlets used to manage components affecting the MSH engine.";
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06005264 RID: 21092 RVA: 0x001B7C9F File Offset: 0x001B5E9F
		public override string DescriptionResource
		{
			get
			{
				return "CoreMshSnapInResources,Description";
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x001B7CA6 File Offset: 0x001B5EA6
		public override string[] Types
		{
			get
			{
				return this._types;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06005266 RID: 21094 RVA: 0x001B7CAE File Offset: 0x001B5EAE
		public override string[] Formats
		{
			get
			{
				return this._formats;
			}
		}

		// Token: 0x04002A3E RID: 10814
		private string[] _types = new string[]
		{
			"types.ps1xml"
		};

		// Token: 0x04002A3F RID: 10815
		private string[] _formats = new string[]
		{
			"Certificate.format.ps1xml",
			"DotNetTypes.format.ps1xml",
			"FileSystem.format.ps1xml",
			"Help.format.ps1xml",
			"HelpV3.format.ps1xml",
			"PowerShellCore.format.ps1xml",
			"PowerShellTrace.format.ps1xml",
			"Registry.format.ps1xml"
		};
	}
}
