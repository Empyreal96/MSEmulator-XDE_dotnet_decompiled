using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000937 RID: 2359
	internal abstract class ControlBase
	{
		// Token: 0x060057A4 RID: 22436 RVA: 0x001C92AC File Offset: 0x001C74AC
		internal static string GetControlShapeName(ControlBase control)
		{
			if (control is TableControlBody)
			{
				return FormatShape.Table.ToString();
			}
			if (control is ListControlBody)
			{
				return FormatShape.List.ToString();
			}
			if (control is WideControlBody)
			{
				return FormatShape.Wide.ToString();
			}
			if (control is ComplexControlBody)
			{
				return FormatShape.Complex.ToString();
			}
			return "";
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x001C930E File Offset: 0x001C750E
		internal virtual ControlBase Copy()
		{
			return this;
		}
	}
}
