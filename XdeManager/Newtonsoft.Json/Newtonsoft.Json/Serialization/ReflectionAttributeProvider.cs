using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000097 RID: 151
	public class ReflectionAttributeProvider : IAttributeProvider
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x00023EC5 File Offset: 0x000220C5
		public ReflectionAttributeProvider(object attributeProvider)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			this._attributeProvider = attributeProvider;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00023EDF File Offset: 0x000220DF
		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, null, inherit);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00023EEE File Offset: 0x000220EE
		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
		}

		// Token: 0x040002C3 RID: 707
		private readonly object _attributeProvider;
	}
}
