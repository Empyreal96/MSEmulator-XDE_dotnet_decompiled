using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000076 RID: 118
	public interface IAttributeProvider
	{
		// Token: 0x06000668 RID: 1640
		IList<Attribute> GetAttributes(bool inherit);

		// Token: 0x06000669 RID: 1641
		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}
