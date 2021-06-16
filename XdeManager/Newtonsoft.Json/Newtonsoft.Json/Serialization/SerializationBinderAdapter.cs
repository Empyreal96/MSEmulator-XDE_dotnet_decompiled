using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000099 RID: 153
	internal class SerializationBinderAdapter : ISerializationBinder
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x00023FF4 File Offset: 0x000221F4
		public SerializationBinderAdapter(SerializationBinder serializationBinder)
		{
			this.SerializationBinder = serializationBinder;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00024003 File Offset: 0x00022203
		public Type BindToType(string assemblyName, string typeName)
		{
			return this.SerializationBinder.BindToType(assemblyName, typeName);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00024012 File Offset: 0x00022212
		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
		}

		// Token: 0x040002C5 RID: 709
		public readonly SerializationBinder SerializationBinder;
	}
}
