using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000874 RID: 2164
	[Serializable]
	public class HaltCommandException : SystemException
	{
		// Token: 0x060052EF RID: 21231 RVA: 0x001B9889 File Offset: 0x001B7A89
		public HaltCommandException() : base(StringUtil.Format(AutomationExceptions.HaltCommandException, new object[0]))
		{
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x001B98A1 File Offset: 0x001B7AA1
		public HaltCommandException(string message) : base(message)
		{
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x001B98AA File Offset: 0x001B7AAA
		public HaltCommandException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x001B98B4 File Offset: 0x001B7AB4
		protected HaltCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
