using System;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000949 RID: 2377
	public abstract class PSControl
	{
		// Token: 0x060057BC RID: 22460
		internal abstract void WriteToXML(XmlWriter _writer, bool exportScriptBlock);

		// Token: 0x060057BD RID: 22461
		internal abstract bool SafeForExport();
	}
}
