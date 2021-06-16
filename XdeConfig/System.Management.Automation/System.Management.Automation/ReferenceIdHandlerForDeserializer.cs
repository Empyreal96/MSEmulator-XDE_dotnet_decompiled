using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000455 RID: 1109
	internal class ReferenceIdHandlerForDeserializer<T> where T : class
	{
		// Token: 0x0600308F RID: 12431 RVA: 0x00109C17 File Offset: 0x00107E17
		internal void SetRefId(T o, string refId, bool duplicateRefIdsAllowed)
		{
			this.refId2object[refId] = o;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x00109C28 File Offset: 0x00107E28
		internal T GetReferencedObject(string refId)
		{
			T result;
			if (this.refId2object.TryGetValue(refId, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x04001A28 RID: 6696
		private readonly Dictionary<string, T> refId2object = new Dictionary<string, T>();
	}
}
