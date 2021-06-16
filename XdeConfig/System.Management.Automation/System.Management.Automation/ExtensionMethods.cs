using System;

namespace System.Management.Automation
{
	// Token: 0x02000875 RID: 2165
	internal static class ExtensionMethods
	{
		// Token: 0x060052F3 RID: 21235 RVA: 0x001B98BE File Offset: 0x001B7ABE
		public static void SafeInvoke(this EventHandler eventHandler, object sender, EventArgs eventArgs)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, eventArgs);
			}
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x001B98CB File Offset: 0x001B7ACB
		public static void SafeInvoke<T>(this EventHandler<T> eventHandler, object sender, T eventArgs) where T : EventArgs
		{
			if (eventHandler != null)
			{
				eventHandler(sender, eventArgs);
			}
		}
	}
}
