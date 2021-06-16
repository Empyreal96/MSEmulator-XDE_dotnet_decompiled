using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000079 RID: 121
	public interface ISerializationBinder
	{
		// Token: 0x0600066F RID: 1647
		Type BindToType(string assemblyName, string typeName);

		// Token: 0x06000670 RID: 1648
		void BindToName(Type serializedType, out string assemblyName, out string typeName);
	}
}
