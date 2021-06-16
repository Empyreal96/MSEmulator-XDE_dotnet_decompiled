using System;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A4C RID: 2636
	internal static class PSNegotiationHandler
	{
		// Token: 0x06006990 RID: 27024 RVA: 0x00213F74 File Offset: 0x00212174
		internal static CimInstance CreatePSNegotiationData(Version powerShellVersion)
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance("PS_NegotiationData");
			CimProperty newItem = InternalMISerializer.CreateCimProperty("PSVersion", powerShellVersion.ToString(), CimType.String);
			cimInstance.CimInstanceProperties.Add(newItem);
			return cimInstance;
		}
	}
}
