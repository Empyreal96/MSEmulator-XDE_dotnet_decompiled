using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000473 RID: 1139
	public sealed class PathInfoStack : Stack<PathInfo>
	{
		// Token: 0x060032A5 RID: 12965 RVA: 0x00114038 File Offset: 0x00112238
		internal PathInfoStack(string stackName, Stack<PathInfo> locationStack)
		{
			if (locationStack == null)
			{
				throw PSTraceSource.NewArgumentNullException("locationStack");
			}
			if (string.IsNullOrEmpty(stackName))
			{
				throw PSTraceSource.NewArgumentException("stackName");
			}
			this.stackName = stackName;
			PathInfo[] array = new PathInfo[locationStack.Count];
			locationStack.CopyTo(array, 0);
			for (int i = array.Length - 1; i >= 0; i--)
			{
				base.Push(array[i]);
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x001140A0 File Offset: 0x001122A0
		public string Name
		{
			get
			{
				return this.stackName;
			}
		}

		// Token: 0x04001A85 RID: 6789
		private string stackName;
	}
}
