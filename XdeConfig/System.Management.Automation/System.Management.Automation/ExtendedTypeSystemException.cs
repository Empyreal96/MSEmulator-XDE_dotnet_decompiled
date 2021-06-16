using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016A RID: 362
	[Serializable]
	public class ExtendedTypeSystemException : RuntimeException
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x00073FC6 File Offset: 0x000721C6
		public ExtendedTypeSystemException() : base(typeof(ExtendedTypeSystemException).FullName)
		{
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00073FDD File Offset: 0x000721DD
		public ExtendedTypeSystemException(string message) : base(message)
		{
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00073FE6 File Offset: 0x000721E6
		public ExtendedTypeSystemException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00073FF0 File Offset: 0x000721F0
		internal ExtendedTypeSystemException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(StringUtil.Format(resourceString, arguments), innerException)
		{
			base.SetErrorId(errorId);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00074008 File Offset: 0x00072208
		protected ExtendedTypeSystemException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
