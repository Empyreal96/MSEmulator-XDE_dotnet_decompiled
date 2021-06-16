using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000078 RID: 120
	public interface IReferenceResolver
	{
		// Token: 0x0600066B RID: 1643
		object ResolveReference(object context, string reference);

		// Token: 0x0600066C RID: 1644
		string GetReference(object context, object value);

		// Token: 0x0600066D RID: 1645
		bool IsReferenced(object context, object value);

		// Token: 0x0600066E RID: 1646
		void AddReference(object context, string reference, object value);
	}
}
