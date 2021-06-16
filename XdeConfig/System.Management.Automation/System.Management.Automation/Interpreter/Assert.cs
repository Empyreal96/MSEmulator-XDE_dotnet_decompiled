using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000757 RID: 1879
	internal static class Assert
	{
		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x001890D0 File Offset: 0x001872D0
		internal static Exception Unreachable
		{
			get
			{
				return new InvalidOperationException("Code supposed to be unreachable");
			}
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x001890DC File Offset: 0x001872DC
		[Conditional("DEBUG")]
		public static void NotNull(object var)
		{
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x001890DE File Offset: 0x001872DE
		[Conditional("DEBUG")]
		public static void NotNull(object var1, object var2)
		{
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x001890E0 File Offset: 0x001872E0
		[Conditional("DEBUG")]
		public static void NotNull(object var1, object var2, object var3)
		{
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x001890E4 File Offset: 0x001872E4
		[Conditional("DEBUG")]
		public static void NotNullItems<T>(IEnumerable<T> items) where T : class
		{
			foreach (T t in items)
			{
			}
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x00189128 File Offset: 0x00187328
		[Conditional("DEBUG")]
		public static void NotEmpty(string str)
		{
		}
	}
}
