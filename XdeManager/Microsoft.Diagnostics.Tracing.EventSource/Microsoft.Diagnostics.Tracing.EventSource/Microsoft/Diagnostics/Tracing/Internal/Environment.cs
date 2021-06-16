using System;
using System.Resources;
using Microsoft.Reflection;

namespace Microsoft.Diagnostics.Tracing.Internal
{
	// Token: 0x02000072 RID: 114
	internal static class Environment
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000F30F File Offset: 0x0000D50F
		public static int TickCount
		{
			get
			{
				return Environment.TickCount;
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000F318 File Offset: 0x0000D518
		public static string GetResourceString(string key, params object[] args)
		{
			string @string = Environment.rm.GetString(key);
			if (@string != null)
			{
				return string.Format(@string, args);
			}
			string text = string.Empty;
			foreach (object obj in args)
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				text += obj.ToString();
			}
			return key + " (" + text + ")";
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000F392 File Offset: 0x0000D592
		public static string GetRuntimeResourceString(string key, params object[] args)
		{
			return Environment.GetResourceString(key, args);
		}

		// Token: 0x04000147 RID: 327
		public static readonly string NewLine = Environment.NewLine;

		// Token: 0x04000148 RID: 328
		private static ResourceManager rm = new ResourceManager("Microsoft.Diagnostics.Tracing.Messages", typeof(Environment).Assembly());
	}
}
