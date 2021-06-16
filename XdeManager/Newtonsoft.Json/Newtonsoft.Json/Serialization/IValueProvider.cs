using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007B RID: 123
	public interface IValueProvider
	{
		// Token: 0x06000673 RID: 1651
		void SetValue(object target, object value);

		// Token: 0x06000674 RID: 1652
		object GetValue(object target);
	}
}
