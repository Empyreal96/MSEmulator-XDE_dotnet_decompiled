using System;

namespace System.Management.Automation
{
	// Token: 0x0200063F RID: 1599
	internal static class CharOps
	{
		// Token: 0x06004552 RID: 17746 RVA: 0x001736AE File Offset: 0x001718AE
		internal static object CompareStringIeq(char lhs, string rhs)
		{
			if (rhs.Length != 1)
			{
				return Boxed.False;
			}
			return CharOps.CompareIeq(lhs, rhs[0]);
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x001736CC File Offset: 0x001718CC
		internal static object CompareStringIne(char lhs, string rhs)
		{
			if (rhs.Length != 1)
			{
				return Boxed.True;
			}
			return CharOps.CompareIne(lhs, rhs[0]);
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x001736EC File Offset: 0x001718EC
		internal static object CompareIeq(char lhs, char rhs)
		{
			char c = char.ToUpperInvariant(lhs);
			char c2 = char.ToUpperInvariant(rhs);
			if (c != c2)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x00173718 File Offset: 0x00171918
		internal static object CompareIne(char lhs, char rhs)
		{
			char c = char.ToUpperInvariant(lhs);
			char c2 = char.ToUpperInvariant(rhs);
			if (c == c2)
			{
				return Boxed.False;
			}
			return Boxed.True;
		}
	}
}
