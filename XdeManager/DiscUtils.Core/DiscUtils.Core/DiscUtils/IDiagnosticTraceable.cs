using System;
using System.IO;

namespace DiscUtils
{
	// Token: 0x0200001A RID: 26
	public interface IDiagnosticTraceable
	{
		// Token: 0x060000F5 RID: 245
		void Dump(TextWriter writer, string linePrefix);
	}
}
