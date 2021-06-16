using System;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200091D RID: 2333
	internal static class EnumerableExpansionConversion
	{
		// Token: 0x06005783 RID: 22403 RVA: 0x001C8FD5 File Offset: 0x001C71D5
		internal static bool Convert(string expansionString, out EnumerableExpansion expansion)
		{
			expansion = EnumerableExpansion.EnumOnly;
			if (string.Equals(expansionString, "CoreOnly", StringComparison.OrdinalIgnoreCase))
			{
				expansion = EnumerableExpansion.CoreOnly;
				return true;
			}
			if (string.Equals(expansionString, "EnumOnly", StringComparison.OrdinalIgnoreCase))
			{
				expansion = EnumerableExpansion.EnumOnly;
				return true;
			}
			if (string.Equals(expansionString, "Both", StringComparison.OrdinalIgnoreCase))
			{
				expansion = EnumerableExpansion.Both;
				return true;
			}
			return false;
		}

		// Token: 0x04002E95 RID: 11925
		internal const string CoreOnlyString = "CoreOnly";

		// Token: 0x04002E96 RID: 11926
		internal const string EnumOnlyString = "EnumOnly";

		// Token: 0x04002E97 RID: 11927
		internal const string BothString = "Both";
	}
}
