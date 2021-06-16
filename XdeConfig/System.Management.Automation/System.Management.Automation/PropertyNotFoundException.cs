using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200016E RID: 366
	[Serializable]
	public class PropertyNotFoundException : ExtendedTypeSystemException
	{
		// Token: 0x0600128D RID: 4749 RVA: 0x000740D5 File Offset: 0x000722D5
		public PropertyNotFoundException() : base(typeof(PropertyNotFoundException).FullName)
		{
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x000740EC File Offset: 0x000722EC
		public PropertyNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000740F5 File Offset: 0x000722F5
		public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x000740FF File Offset: 0x000722FF
		internal PropertyNotFoundException(string errorId, Exception innerException, string resourceString, params object[] arguments) : base(errorId, innerException, resourceString, arguments)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0007410C File Offset: 0x0007230C
		protected PropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
