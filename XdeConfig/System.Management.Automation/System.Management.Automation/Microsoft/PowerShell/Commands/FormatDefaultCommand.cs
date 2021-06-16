using System;
using System.Management.Automation;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020004D5 RID: 1237
	[Cmdlet("Format", "Default")]
	public class FormatDefaultCommand : FrontEndCommandBase
	{
		// Token: 0x06003608 RID: 13832 RVA: 0x0012520D File Offset: 0x0012340D
		public FormatDefaultCommand()
		{
			this.implementation = new InnerFormatShapeCommand(FormatShape.Undefined);
		}
	}
}
