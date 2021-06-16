using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200072F RID: 1839
	internal sealed class RuntimeVariables : IRuntimeVariables
	{
		// Token: 0x06004A6C RID: 19052 RVA: 0x001874AA File Offset: 0x001856AA
		private RuntimeVariables(IStrongBox[] boxes)
		{
			this._boxes = boxes;
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x001874B9 File Offset: 0x001856B9
		int IRuntimeVariables.Count
		{
			get
			{
				return this._boxes.Length;
			}
		}

		// Token: 0x17000F9E RID: 3998
		object IRuntimeVariables.this[int index]
		{
			get
			{
				return this._boxes[index].Value;
			}
			set
			{
				this._boxes[index].Value = value;
			}
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x001874E2 File Offset: 0x001856E2
		internal static IRuntimeVariables Create(IStrongBox[] boxes)
		{
			return new RuntimeVariables(boxes);
		}

		// Token: 0x0400240E RID: 9230
		private readonly IStrongBox[] _boxes;
	}
}
