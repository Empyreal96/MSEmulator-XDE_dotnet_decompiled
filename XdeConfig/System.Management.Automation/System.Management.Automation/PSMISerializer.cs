using System;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A44 RID: 2628
	internal class PSMISerializer
	{
		// Token: 0x06006980 RID: 27008 RVA: 0x00213C82 File Offset: 0x00211E82
		internal PSMISerializer()
		{
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x00213C8A File Offset: 0x00211E8A
		public static CimInstance Serialize(object source)
		{
			return PSMISerializer.Serialize(source, PSMISerializer.mshDefaultMISerializationDepth);
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x00213C98 File Offset: 0x00211E98
		public static CimInstance Serialize(object source, int serializationDepth)
		{
			MISerializer miserializer = new MISerializer(serializationDepth);
			return miserializer.Serialize(source);
		}

		// Token: 0x04003282 RID: 12930
		private static int mshDefaultMISerializationDepth = 1;
	}
}
